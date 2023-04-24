using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class RegistrationController : Controller {
	private TravelRewardsContext _dbContext { get; set; }
	private string _passwordSalt { get; set; }

	#region Class Constructor(s)
	public RegistrationController(
		IOptions<AppSettings> appSettingsInjection,
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;

		_passwordSalt = "default";
		if (appSettingsInjection.Value.AccountConfig != null)
			_passwordSalt = appSettingsInjection.Value.AccountConfig.AccountPasswordSalt ?? "default";
	}
	#endregion Class Constructor(s)

	#region Helper Method(s)
	// Salting the password and Hashing
	private string HashingAndSalt(string plainText) {
		string passwordHashed = BCrypt.Net.BCrypt.EnhancedHashPassword(plainText + _passwordSalt);

		//To Verify if the password is correct.
		//bool verified = false;
		//verified = BCrypt.Net.BCrypt.EnhancedVerify(plainText+salt, passwordHashed);

		return passwordHashed;
	}

	//Account Creation, insert into Database.
	private IActionResult CreateAccount([FromBody] RegisterAccount register) {
		//Input validation
		string inputUsername = register.Username ?? "";
		string inputPassword = register.Password ?? "";
		if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(inputUsername))
			return StatusCode(400, "0: Malformed body content detected");

		// Checks if user exists
		Account? existingAccount = _dbContext.Accounts
			.Where(x => !x.AccDisable)
			.Where(x => x.AccUsername
				.Contains(inputUsername))
			.FirstOrDefault();

		//Detected existing account
		if (existingAccount != null)
			return StatusCode(401, new ErrorExceptionHelper().Exception401(8));

		Guid guid = Guid.NewGuid();
		DateTime dateTime = DateTime.Now;

		bool convertedMobilityImpaired = false;
		if (register.MobilityImpaired == 0 || register.MobilityImpaired == 1)
			convertedMobilityImpaired = Convert.ToBoolean(register.MobilityImpaired);
		else
			convertedMobilityImpaired = Convert.ToBoolean(register.MobilityImpaired = 0);

		//Generate entries for new account
		Account acc = new Account() {
			AccId = guid,
			AccUsername = inputUsername,
			AccPassword = inputPassword,
			AccDisable = false,
			AccRole = register.Role,
			CreatedAt = dateTime,
			CreatedBy = register.CreatedBy
		};
		AccountDetail accDetail = new AccountDetail() {
			AccId = guid,
			AccDetailsFirstName = register.FirstName ?? "",
			AccDetailsLastName = register.LastName ?? "",
			AccDetailsGender = register.Gender,
			//AccDetailsProfilePicture = register.ProfilePicture ?? Encoding.UTF8.GetBytes(""),
			AccDetailsAge = register.Age,
			AccDetailsTotalPoints = 0,
			AccDetailsPhoneCountryCode = register.PhoneCountryCode ?? "",
			AccDetailsPhoneNumber = register.PhoneNumber ?? "",
			AccDetailsAddressCountry = register.Country ?? "",
			AccDetailsAddressCity = register.City ?? "",
			AccDetailsAddressZipCode = register.ZipCode ?? "",
			AccDetailsAddressStreet = register.StreetAddress ?? "",
			CreatedAt = dateTime,
			CreatedBy = register.CreatedBy
		};
		AccountEmployment accEmployment = new AccountEmployment() {
			AccId = guid,
			AccEmpStatus = register.EmploymentStatus ?? "",
			AccEmpOccupation = register.Occupation ?? "",
			AccEmpLocation = register.EmploymentLocation ?? "",
			AccEmpStartTime = register.EmploymentStartTime ?? 0,
			AccEmpEndTime = register.EmploymentEndTime ?? 0,
			AccEmpAnnualSalary = register.EmploymentAnnualSalary ?? 0,
			CreatedAt = dateTime,
			CreatedBy = register.CreatedBy
		};
		AccountExtra accExtra = new AccountExtra() {
			AccId = guid,
			AccExtraDriverLicense = register.DriverLicense ?? "",
			AccExtraMobilityImpaired = convertedMobilityImpaired,
			AccExtraHouseholdPosition = register.HouseholdPosition ?? "",
			AccExtraNumberOfVehicles = register.NoOfVehicles ?? 0,
			CreatedAt = dateTime,
			CreatedBy = register.CreatedBy
		};
		AccountPointsTxn accPointsTxn = new AccountPointsTxn() {
			AccId = guid,
			AccPointsTxnAmt = 0,
			CreatedAt = dateTime,
			CreatedBy = register.CreatedBy
		};

		//Insert entries into database
		try {
			_dbContext.Accounts.Add(acc);
			_dbContext.AccountDetails.Add(accDetail);
			_dbContext.AccountEmployments.Add(accEmployment);
			_dbContext.AccountExtras.Add(accExtra);
			_dbContext.AccountPointsTxns.Add(accPointsTxn);

			_dbContext.SaveChanges();
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}

		return Ok("User " + register.Username + " Created. Role: " + register.Role);
	}
	#endregion Helper Method(s)

	#region API Method(s)
	//Registration API for Mobile User
	[HttpPost, Route("MobileUser"), AllowAnonymous]
	public IActionResult RegisterMobileUser([FromBody] RegisterAccount registration) {
		//Input validation
		string inputUsername = registration.Username ?? "";
		string inputPassword = registration.Password ?? "";
		if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(inputUsername))
			return StatusCode(400, "Malformed body content detected");

		string SaltedPwd = HashingAndSalt(inputPassword);
		registration.Password = SaltedPwd;
		registration.Role = (int)LoginRoles.MobileUser;
		registration.CreatedBy = "[REGISTRATION MOBILE API]";

		return CreateAccount(registration);
	}

	//Registration API for Dashboard User
	[HttpPost, Route("DashboardUser"), Authorize(Roles = "Admin")]
	public IActionResult RegisterDashboardUser([FromBody] RegisterAccount registration) {
		//Input validation
		string inputUsername = registration.Username ?? "";
		string inputPassword = registration.Password ?? "";
		if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(inputUsername))
			return StatusCode(400, "Malformed body content detected");

		string SaltedPwd = HashingAndSalt(inputPassword);
		registration.Password = SaltedPwd;
		registration.Role = (int)LoginRoles.DashboardUser;
		registration.CreatedBy = "[REGISTRATION DASHBOARD API]";

		return CreateAccount(registration);
	}

	//Check if user exists
	[HttpPost, Route("UserExist"), AllowAnonymous]
	public IActionResult IsUserExist([FromBody] UserExists userExists) {
		string inputUsername = userExists.Username ?? "";
		if (string.IsNullOrEmpty(inputUsername))
			return StatusCode(400, "Error: Missing username input.");

		Account? existingAccount = _dbContext.Accounts
			.Where(x => !x.AccDisable)
			.Where(x => x.AccUsername
				.Contains(inputUsername))
			.FirstOrDefault();

		if (existingAccount != null)
			return StatusCode(400, "Account Already Exists.");

		return Ok("Ok");
	}
	#endregion API Method(s)
}