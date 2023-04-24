using ActivitySurveyAppForSmartCityPlanning;
using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ActvitySurveyAppForSmartCityPlanning.Services;

public class LoginService {
	private IOptions<AppSettings> _appSettings { get; set; }

	#region Class Constructor(s)
	public LoginService(IOptions<AppSettings> appSettingsInjection) {
		_appSettings = appSettingsInjection;
	}
	#endregion Class Constructor(s)

	public string GenerateJwt(Account account) {
		try {
			//Append permissions to claims
			List<Claim> claims = new List<Claim>();
			for (int i = account.AccRole; i > -1; i--) {
				claims.Add(new Claim(ClaimTypes.Role, (Enum.GetName(typeof(LoginRoles), i)) ?? ""));
			}

			//Validate JwtConfig existance in appsettings.json
			JwtConfig? jwtConfig = _appSettings.Value.JwtConfig;
			if (jwtConfig == null) throw new Exception("Invalid appsettings.json");

			//Generate signing crednetials
			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(jwtConfig.JwtSymmetricKey ?? "default"));
			SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			//Append user data to claims
			claims.Add(new Claim(ClaimTypes.Name, account.AccUsername));
			claims.Add(new Claim(ClaimTypes.NameIdentifier, account.AccId.ToString()));

			//Generate claims
			JwtSecurityToken token = new JwtSecurityToken(
				jwtConfig.JwtIssuer ?? "default",
				jwtConfig.JwtAudience ?? "default",
				claims.ToArray(),
				DateTime.UtcNow,
				expires: DateTime.Now.AddYears(jwtConfig.JwtExpiryFromNow ?? 10),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		catch (Exception) {
			throw;
		}
	}
}