using Flashcards.Core.Domain.Identity;
using Flashcards.Core.DTO.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.WebAPI.Controllers
{
	[AllowAnonymous]
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
    }
}
