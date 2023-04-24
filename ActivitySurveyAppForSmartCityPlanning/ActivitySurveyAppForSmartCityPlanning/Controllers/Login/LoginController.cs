using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.Services;
using ActvitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers.Login;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class LoginController : Controller {
	private IOptions<AppSettings> _appSettings { get; set; }
	private TravelRewardsContext _dbContext { get; set; }
	private LoginService _tokenService { get; set; }
	private string _passwordSalt { get; set; }

	#region Class Constructor(s)
	public LoginController(
		IOptions<AppSettings> appSettingsInjection,
		TravelRewardsContext dbContextInjection) {
		_appSettings = appSettingsInjection;
		_dbContext = dbContextInjection;
		_tokenService = new LoginService(appSettingsInjection);

		_passwordSalt = "default";
		if (appSettingsInjection.Value.AccountConfig != null)
			_passwordSalt = appSettingsInjection.Value.AccountConfig.AccountPasswordSalt ?? "default";
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	[HttpPost, AllowAnonymous]
	public async Task<IActionResult> Login([FromBody] LoginCredentials loginCredentials) {
		try {
			//Validate request body content
			if (loginCredentials.Username == null || loginCredentials.Password == null)
				return StatusCode(400, "0: Invalid request body content");

			//Try retrieve account
			Account? targetAccount = await _dbContext.Accounts
				.Where(x => x.AccDisable == false)
				.Where(y => string.Equals(y.AccUsername, loginCredentials.Username))
				.FirstOrDefaultAsync();

			//Check if username is valid
			if (targetAccount == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(6));

			//Check if password is valid
			if (!BCrypt.Net.BCrypt.EnhancedVerify(
				loginCredentials.Password + _passwordSalt,
				targetAccount.AccPassword))
				return StatusCode(401, new ErrorExceptionHelper().Exception401(6));

			//Check for any existing token session, for mobile role only
			if (targetAccount.AccRole == 0) {
				if (!string.IsNullOrEmpty(targetAccount.AccCurrentSession))
					return StatusCode(401, new ErrorExceptionHelper().Exception401(5));
			}

			//Generate JWT
			string jwt = _tokenService.GenerateJwt(targetAccount);

			//Update database of current session's token
			targetAccount.AccCurrentSession = jwt;

			//Trigger save on database side
			await _dbContext.SaveChangesAsync();

			return Ok(jwt);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}
	[HttpPost, AllowAnonymous, Route("mobile")]
	public async Task<IActionResult> MobileLogin([FromBody] LoginCredentials loginCredentials) {
		try {
			//Validate request body content
			if (loginCredentials.Username == null || loginCredentials.Password == null)
				return StatusCode(400, "0: Invalid request body content");
			if (loginCredentials.MobileId == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(7));

			//Try retrieve account
			Account? targetAccount = await _dbContext.Accounts
				.Where(x => x.AccDisable == false)
				.Where(y => string.Equals(y.AccUsername, loginCredentials.Username))
				.FirstOrDefaultAsync();

			//Check if username is valid
			if (targetAccount == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(6));

			//Check if password is valid
			if (!BCrypt.Net.BCrypt.EnhancedVerify(
				loginCredentials.Password + _passwordSalt,
				targetAccount.AccPassword))
				return StatusCode(401, new ErrorExceptionHelper().Exception401(6));

			//Check for any existing token session, for mobile role only
			if (targetAccount.AccRole == 0) {
				if (!string.IsNullOrEmpty(targetAccount.AccCurrentSession)) {
					if (!string.Equals(targetAccount.AccCurrentSession, loginCredentials.MobileId)) {
						return StatusCode(401, new ErrorExceptionHelper().Exception401(5));
					}
				}
			}

			//Generate JWT
			string jwt = _tokenService.GenerateJwt(targetAccount);

			//Update database of current session's Id
			targetAccount.AccCurrentSession = loginCredentials.MobileId;

			//Trigger save on database side
			await _dbContext.SaveChangesAsync();

			return Ok(jwt);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	[HttpPost, Route("Logout"), Authorize]
	public async Task<IActionResult> Logout() {
		try {
			//Check if token is a valid identity token
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Retrieve account Id from token
			Claim[] claims = identity.Claims.ToArray();
			string? accId = claims
				.Where(x => x.Type == ClaimTypes.NameIdentifier)
				.Select(x => x.Value)
				.SingleOrDefault();

			//Check if token contains account Id
			if (string.IsNullOrEmpty(accId))
				return StatusCode(401, new ErrorExceptionHelper().Exception401(1));

			//Try retrieve account
			Account? targetAccount = await _dbContext.Accounts
				.Where(x => string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();

			//Check account Id is valid
			if (targetAccount == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(2));

			//Remove database of current session's token
			targetAccount.AccCurrentSession = null;

			//Trigger save in database side
			await _dbContext.SaveChangesAsync();

			return Ok();
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	[HttpPost, Route("LogoutWithCred"), AllowAnonymous]
	public async Task<IActionResult> LogoutWithCred([FromBody] LoginCredentials loginCredentials) {
		try {
			//Validate request body content
			if (loginCredentials.Username == null || loginCredentials.Password == null)
				return StatusCode(400, new ErrorExceptionHelper().Exception401(0));

			//Try retrieve account
			Account? targetAccount = _dbContext.Accounts
				.Where(x => x.AccDisable == false)
				.Where(y => string.Equals(y.AccUsername, loginCredentials.Username))
				.FirstOrDefault();

			//Check if username is valid
			if (targetAccount == null)
				return StatusCode(400, new ErrorExceptionHelper().Exception401(1));

			//Check if password is valid
			if (!BCrypt.Net.BCrypt.EnhancedVerify(
				loginCredentials.Password + _passwordSalt,
				targetAccount.AccPassword))
				return StatusCode(400, new ErrorExceptionHelper().Exception401(4));

			//Update database of current session's token
			targetAccount.AccCurrentSession = null;

			//Trigger save in database side
			await _dbContext.SaveChangesAsync();

			return Ok();
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	//Check authenticate JWT token for User role
	[HttpGet, Route("AuthMobileUser"), Authorize(Roles = "MobileUser")]
	public IActionResult AuthMobileUser() {
		ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
		if (identity == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

		try {
			Claim[] claims = identity.Claims.ToArray();
			string? username = claims
				.Where(x => x.Type == ClaimTypes.Name)
				.Select(x => x.Value)
				.SingleOrDefault();

			return Ok("Welcome MobileUser, " + username);
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}
	//Check authenticate JWT token for Admin role
	[HttpGet, Route("AuthDashboardUser"), Authorize(Roles = "DashboardUser")]
	public IActionResult AuthDashboardUser() {
		ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
		if (identity == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

		try {
			Claim[] claims = identity.Claims.ToArray();
			string? username = claims
				.Where(x => x.Type == ClaimTypes.Name)
				.Select(x => x.Value)
				.SingleOrDefault();

			return Ok("Welcome DashboardUser, " + username);
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}
	//Check authenticate JWT token for Admin role
	[HttpGet, Route("AuthAdmin"), Authorize(Roles = "Admin")]
	public IActionResult AuthAdmin() {
		ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
		if (identity == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

		try {
			Claim[] claims = identity.Claims.ToArray();
			string? username = claims
				.Where(x => x.Type == ClaimTypes.Name)
				.Select(x => x.Value)
				.SingleOrDefault();

			return Ok("Welcome Admin, " + username);
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}
	#endregion API Method(s)
}