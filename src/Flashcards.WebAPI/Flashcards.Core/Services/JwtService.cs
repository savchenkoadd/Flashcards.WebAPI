using Flashcards.Core.Domain.Identity;
using Flashcards.Core.DTO.JWT;
using Flashcards.Core.ServiceContracts;
using Flashcards.Core.Services.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Flashcards.Core.Services
{
	public class JwtService : IJwtService
	{
		private const string EXPIRATION_CONFIG_PATH = "JwtOptions:ExpirationMinutes";
		private const string SECRET_KEY_CONFIG_PATH = "Jwt:Key";
		private const string ISSUER_CONFIG_PATH = "Jwt:Issuer";
		private const string AUDIENCE_CONFIG_PATH = "Jwt:Audience";

		private readonly IConfiguration _configuration;
		private readonly JwtHelper _jwtHelper;

		public JwtService(IConfiguration configuration)
		{
			_configuration = configuration;
			_jwtHelper = InitializeJwtHelper();
		}

		public async Task<AuthenticationResponse> CreateJwtToken(ApplicationUser? applicationUser)
		{
			await ValidationHelper.ValidateObjects(applicationUser);

			DateTime expiration = await _jwtHelper.CalculateExpirationTime();

			Claim[] claims = await _jwtHelper.GenerateClaims(applicationUser!);

			SymmetricSecurityKey securityKey = await _jwtHelper.GenerateSecurityKey();

			JwtSecurityToken jwtSecurityToken = await _jwtHelper.GetJwtSecurityTokenWithHmacSha256(securityKey, claims, expiration);

			var token = await _jwtHelper.GetTokenString(jwtSecurityToken);

			return await _jwtHelper.GenerateAuthenticationResponse(applicationUser, expiration, token);
		}

		private JwtHelper InitializeJwtHelper()
		{
			return new JwtHelper(
					_configuration,
					expirationConfigPath: EXPIRATION_CONFIG_PATH,
					secretKeyConfigPath: SECRET_KEY_CONFIG_PATH,
					issuerConfigPath: ISSUER_CONFIG_PATH,
					audienceConfigPath: AUDIENCE_CONFIG_PATH
				);
		}
	}
}
