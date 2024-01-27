using Flashcards.Core.Domain.Identity;
using Flashcards.Core.DTO.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.WebAPI.Controllers
{
	[Route("api")]
	[ApiController]
	[ApiVersion("1.1")]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(
				UserManager<ApplicationUser> userManager,
				SignInManager<ApplicationUser> signInManager,
				RoleManager<ApplicationRole> roleManager
			)
        {
            _roleManager = roleManager;
			_userManager = userManager;
			_signInManager = signInManager;
        }

		//POST: /api/register
		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<ActionResult<ApplicationUser>> Register(RegisterDTO? registerDTO)
		{
			if (registerDTO is null)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(value => value.Errors).Select(e => e.ErrorMessage));

				return Problem(errorMessage);
			}

			ApplicationUser user = new ApplicationUser()
			{
				Email = registerDTO.Email,
				PhoneNumber = registerDTO.PhoneNumber,
				UserName = registerDTO.Email,
				PersonName = registerDTO.PersonName
			};

			IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, isPersistent: true);

				return Ok(user);
			}
			else
			{
				string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

				return Problem(errorMessage);
			}
		}

		//POST: /api/login
		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> Login(LoginDTO? loginDTO)
		{
			if (loginDTO is null)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(value => value.Errors).Select(e => e.ErrorMessage));

				return Problem(errorMessage);
			}

			var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: true, lockoutOnFailure: false);

			if (result.Succeeded)
			{
				ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

				if (user is null)
				{
					return Problem(statusCode: 400, detail: "The email is not valid.");
				}

				return Ok();
			}
			else
			{
				return Problem(statusCode: 400, detail: "Invalid email or password.");
			}
		}

		//POST: /api/logout
		[HttpGet("[action]")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return Ok();
		}
	}
}
