using Flashcards.Core.Domain.Identity;
using Flashcards.Core.DTO.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Flashcards.Core.Services.Helpers
{
	internal class JwtHelper
	{
		private readonly IConfiguration _configuration;
		private readonly string _expirationConfigPath;
		private readonly string _secretKeyConfigPath;
		private readonly string _issuerConfigPath;
		private readonly string _audienceConfigPath;

		public JwtHelper(IConfiguration configuration, string? expirationConfigPath, string? secretKeyConfigPath, string? issuerConfigPath, string? audienceConfigPath)
		{
			if (string.IsNullOrEmpty(expirationConfigPath) ||
				string.IsNullOrEmpty(secretKeyConfigPath) ||
				string.IsNullOrEmpty(issuerConfigPath) ||
				string.IsNullOrEmpty(audienceConfigPath))
			{
				throw new ArgumentNullException($"Config path cannot be null or empty.");
			}

			_configuration = configuration;
			_expirationConfigPath = expirationConfigPath;
			_secretKeyConfigPath = secretKeyConfigPath;
			_issuerConfigPath = issuerConfigPath;
			_audienceConfigPath = audienceConfigPath;
		}

		internal async Task<DateTime> CalculateExpirationTime()
		{
			return await Task.FromResult(DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration[_expirationConfigPath])));
		}

		internal async Task<Claim[]> GenerateClaims(ApplicationUser? user)
		{
			await ValidationHelper.ValidateObjects(user);

			return new Claim[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

				new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),

				new Claim(ClaimTypes.NameIdentifier, user.Email),

				new Claim(ClaimTypes.NameIdentifier, user.PersonName),
			};
		}

		internal async Task<SymmetricSecurityKey> GenerateSecurityKey()
		{
			return new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_configuration[_secretKeyConfigPath])
				);
		}

		internal async Task<AuthenticationResponse> GenerateAuthenticationResponse(ApplicationUser? applicationUser, DateTime? expiration, string? token)
		{
			await ValidationHelper.ValidateObjects(applicationUser, expiration, token);

			return new AuthenticationResponse { Token = token, Email = applicationUser.Email, PersonName = applicationUser.PersonName, Expiration = expiration.Value };
		}

		internal async Task<string> GetTokenString(SecurityToken jwtSecurityToken)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.WriteToken(jwtSecurityToken);

			if (token is null)
			{
				throw new SecurityTokenEncryptionFailedException();
			}

			return await Task.FromResult(token);
		}

		internal async Task<JwtSecurityToken> GetJwtSecurityTokenWithHmacSha256(SecurityKey securityKey, Claim[] claims, DateTime expiration)
		{
			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			return await Task.FromResult(new JwtSecurityToken(
					_configuration[_issuerConfigPath],
					_configuration[_audienceConfigPath],
					claims,
					expires: expiration,
					signingCredentials: signingCredentials
				));
		}
	}
}
