using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers.Profile;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class ProfileController : Controller {
	private TravelRewardsContext _dbContext;

	#region Class Constructor(s)
	public ProfileController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region Helper Method(s)
	private string? GetAccIdFromToken(Claim[] claims) {
		try {
			string? accId = claims
				.Where(x => x.Type == ClaimTypes.NameIdentifier)
				.Select(x => x.Value)
				.SingleOrDefault();
			return accId;
		}
		catch { throw; }
	}
	#endregion Helper Method(s)

	#region API Method(s)
	[HttpGet, Route("GetProfile"), Authorize]
	public async Task<IActionResult> GetProfile() {
		try {
			//Extract token from request
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Extract claims from token
			Claim[] claims = identity.Claims.ToArray();

			//Extract AccId from claims
			string accId = GetAccIdFromToken(claims) ?? "";
			if (string.IsNullOrEmpty(accId)) return StatusCode(401, new ErrorExceptionHelper().Exception401(1));

			//Retrieve Account from database
			Account? targetAcc = await _dbContext.Accounts
				.Where(x => !x.AccDisable && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAcc == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(2));

			//Package return data
			Profile_Get returnData = new Profile_Get {
				Username = targetAcc.AccUsername,
				Role = targetAcc.AccRole,

				CreatedAt = targetAcc.CreatedAt.ToString(),
				CreatedBy = targetAcc.CreatedBy,
				ModifiedAt = targetAcc.ModifiedAt.ToString(),
				ModifiedBy = targetAcc.ModifiedBy
			};

			return Ok(returnData);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	[HttpGet, Route("GetProfileDetail"), Authorize]
	public async Task<IActionResult> GetProfileDetail() {
		try {
			//Extract token from request
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Extract claims from token
			Claim[] claims = identity.Claims.ToArray();

			//Extract AccId from claims
			string accId = GetAccIdFromToken(claims) ?? "";
			if (string.IsNullOrEmpty(accId)) return StatusCode(401, new ErrorExceptionHelper().Exception401(1));

			//Retrieve Account from database
			Account? targetAcc = await _dbContext.Accounts
				.Where(x => !x.AccDisable && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAcc == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(2));

			//Retrieve AccountDetail from database
			AccountDetail? targetAccDetail = await _dbContext.AccountDetails
				.Where(x => x.DeletedAt == null && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAccDetail == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(3));

			//Package return data
			ProfileDetails_Get returnData = new ProfileDetails_Get {
				FirstName = targetAccDetail.AccDetailsFirstName,
				LastName = targetAccDetail.AccDetailsLastName,
				Gender = targetAccDetail.AccDetailsGender ?? 0,
				Age = targetAccDetail.AccDetailsAge,
				ProfilePicture = targetAccDetail.AccDetailsProfilePicture,
				TotalPoints = targetAccDetail.AccDetailsTotalPoints ?? 0,
				PhoneCountryCode = targetAccDetail.AccDetailsPhoneCountryCode,
				PhoneNumber = targetAccDetail.AccDetailsPhoneNumber,
				Country = targetAccDetail.AccDetailsAddressCountry,
				City = targetAccDetail.AccDetailsAddressCity,
				ZipCode = targetAccDetail.AccDetailsAddressZipCode,
				StreetAddress = targetAccDetail.AccDetailsAddressStreet,

				CreatedAt = targetAccDetail.CreatedAt.ToString(),
				CreatedBy = targetAccDetail.CreatedBy,
				ModifiedAt = targetAccDetail.ModifiedAt.ToString(),
				ModifiedBy = targetAccDetail.ModifiedBy
			};

			return Ok(returnData);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	[HttpGet, Route("GetProfileEmployment"), Authorize]
	public async Task<IActionResult> GetProfileEmployment() {
		try {
			//Extract token from request
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(400, "0:Malformed token detected");

			//Extract claims from token
			Claim[] claims = identity.Claims.ToArray();

			//Extract AccId from claims
			string accId = GetAccIdFromToken(claims) ?? "";
			if (string.IsNullOrEmpty(accId)) return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Retrieve Account from database
			Account? targetAcc = await _dbContext.Accounts
				.Where(x => !x.AccDisable && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAcc == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(1));

			//Retrieve AccountEmployment from database
			AccountEmployment? targetAccEmployment = await _dbContext.AccountEmployments
				.Where(x => x.DeletedAt == null && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAccEmployment == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(3));

			//Package return data
			ProfileEmployment_Get returnData = new ProfileEmployment_Get {
				Status = targetAccEmployment.AccEmpStatus,
				Occupation = targetAccEmployment.AccEmpOccupation,
				Location = targetAccEmployment.AccEmpLocation,
				StartTime = targetAccEmployment.AccEmpStartTime,
				EndTime = targetAccEmployment.AccEmpEndTime,
				AnnualSalary = targetAccEmployment.AccEmpAnnualSalary,

				CreatedAt = targetAccEmployment.CreatedAt.ToString(),
				CreatedBy = targetAccEmployment.CreatedBy,
				ModifiedAt = targetAccEmployment.ModifiedAt.ToString(),
				ModifiedBy = targetAccEmployment.ModifiedBy
			};

			return Ok(returnData);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	[HttpGet, Route("GetProfileExtra"), Authorize]
	public async Task<IActionResult> GetProfileExtra() {
		try {
			//Extract token from request
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(400, "0:Malformed token detected");

			//Extract claims from token
			Claim[] claims = identity.Claims.ToArray();

			//Extract AccId from claims
			string accId = GetAccIdFromToken(claims) ?? "";
			if (string.IsNullOrEmpty(accId)) return StatusCode(401, new ErrorExceptionHelper().Exception401(1));

			//Retrieve Account from database
			Account? targetAcc = await _dbContext.Accounts
				.Where(x => !x.AccDisable && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAcc == null) return StatusCode(401, "2:Invalid AccId detected");

			//Retrieve AccountExtra from database
			AccountExtra? targetAccExtra = await _dbContext.AccountExtras
				.Where(x => x.DeletedAt == null && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAccExtra == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(3));

			//Package return data
			ProfileExtras_Get returnData = new ProfileExtras_Get {
				DriverLicense = targetAccExtra.AccExtraDriverLicense,
				MobilityImpaired = targetAccExtra.AccExtraMobilityImpaired ? 1 : 0,
				HouseholdPosition = targetAccExtra.AccExtraHouseholdPosition,
				NoOfVehicles = targetAccExtra.AccExtraNumberOfVehicles,

				CreatedAt = targetAccExtra.CreatedAt.ToString(),
				CreatedBy = targetAccExtra.CreatedBy,
				ModifiedAt = targetAccExtra.ModifiedAt.ToString(),
				ModifiedBy = targetAccExtra.ModifiedBy
			};

			return Ok(returnData);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	[HttpPost, Route("UpdateProfileDetail"), Authorize]
	public async Task<IActionResult> UpdateProfileDetail([FromBody] ProfileDetails_Post profileDetails) {
		try {
			//Extract token from request
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(400, "0: Malformed token detected");

			//Extract claims from token
			Claim[] claims = identity.Claims.ToArray();

			//Extract AccId from claims
			string accId = GetAccIdFromToken(claims) ?? "";
			if (string.IsNullOrEmpty(accId)) return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Retrieve Account from database
			Account? targetAcc = await _dbContext.Accounts
				.Where(x => !x.AccDisable && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAcc == null) return StatusCode(401, "2: Invalid AccId detected");

			//Retrieve AccountDetail from database
			AccountDetail? targetAccDetail = await _dbContext.AccountDetails
				.Where(x => x.DeletedAt == null && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAccDetail == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(3));

			//Package data
			targetAccDetail.AccDetailsFirstName =
				profileDetails.FirstName ?? targetAccDetail.AccDetailsFirstName;
			targetAccDetail.AccDetailsLastName =
				profileDetails.LastName ?? targetAccDetail.AccDetailsLastName;
			targetAccDetail.AccDetailsGender =
				profileDetails.Gender ?? targetAccDetail.AccDetailsGender;
			targetAccDetail.AccDetailsAge =
				profileDetails.Age ?? targetAccDetail.AccDetailsAge;
			targetAccDetail.AccDetailsPhoneCountryCode =
				profileDetails.PhoneCountryCode ?? targetAccDetail.AccDetailsPhoneCountryCode;
			targetAccDetail.AccDetailsPhoneNumber =
				profileDetails.PhoneNumber ?? targetAccDetail.AccDetailsPhoneNumber;
			targetAccDetail.AccDetailsAddressCountry =
				profileDetails.Country ?? targetAccDetail.AccDetailsAddressCountry;
			targetAccDetail.AccDetailsAddressCity =
				profileDetails.City ?? targetAccDetail.AccDetailsAddressCity;
			targetAccDetail.AccDetailsAddressZipCode =
				profileDetails.ZipCode ?? targetAccDetail.AccDetailsAddressZipCode;
			targetAccDetail.AccDetailsAddressStreet =
				profileDetails.StreetAddress ?? targetAccDetail.AccDetailsAddressStreet;
			targetAccDetail.ModifiedAt = DateTime.Now;
			targetAccDetail.ModifiedBy = targetAcc.AccId.ToString();

			//Save changes to database
			_dbContext.AccountDetails.Update(targetAccDetail);
			_dbContext.SaveChanges();

			return Ok(true);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	[HttpPost, Route("UpdateEmployment"), Authorize]
	public async Task<IActionResult> UpdateEmployment([FromBody] ProfileEmployment_Post profileEmployment) {
		try {
			//Extract token from request
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(400, "0: Malformed token detected");

			//Extract claims from token
			Claim[] claims = identity.Claims.ToArray();

			//Extract AccId from claims
			string accId = GetAccIdFromToken(claims) ?? "";
			if (string.IsNullOrEmpty(accId)) return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Retrieve Account from database
			Account? targetAcc = await _dbContext.Accounts
				.Where(x => !x.AccDisable && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAcc == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(1));

			//Retrieve AccountEmployement from database
			AccountEmployment? targetAccEmployment = await _dbContext.AccountEmployments
				.Where(x => x.DeletedAt == null && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAccEmployment == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(3));

			//Package data
			targetAccEmployment.AccEmpStatus =
				profileEmployment.Status ?? targetAccEmployment.AccEmpStatus;
			targetAccEmployment.AccEmpOccupation =
				profileEmployment.Occupation ?? targetAccEmployment.AccEmpOccupation;
			targetAccEmployment.AccEmpLocation =
				profileEmployment.Location ?? targetAccEmployment.AccEmpLocation;
			targetAccEmployment.AccEmpStartTime =
				profileEmployment.StartTime ?? targetAccEmployment.AccEmpStartTime;
			targetAccEmployment.AccEmpEndTime =
				profileEmployment.EndTime ?? targetAccEmployment.AccEmpEndTime;
			targetAccEmployment.AccEmpAnnualSalary =
				profileEmployment.AnnualSalary ?? targetAccEmployment.AccEmpAnnualSalary;
			targetAccEmployment.ModifiedAt = DateTime.Now;
			targetAccEmployment.ModifiedBy = targetAcc.AccId.ToString();

			//Save changes to database
			_dbContext.AccountEmployments.Update(targetAccEmployment);
			_dbContext.SaveChanges();

			return Ok(true);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	[HttpPost, Route("UpdateExtras"), Authorize]
	public async Task<IActionResult> UpdateExtras([FromBody] ProfileExtras_Post inputProfileExtras) {
		try {
			//Extract token from request
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(400, "0: Malformed token detected");

			//Extract claims from token
			Claim[] claims = identity.Claims.ToArray();

			//Extract AccId from claims
			string accId = GetAccIdFromToken(claims) ?? "";
			if (string.IsNullOrEmpty(accId)) return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Retrieve Account from database
			Account? targetAcc = await _dbContext.Accounts
				.Where(x => !x.AccDisable && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAcc == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(1));

			//Retrieve AccountExtra from database
			AccountExtra? targetAccExtra = await _dbContext.AccountExtras
				.Where(x => x.DeletedAt == null && string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			if (targetAccExtra == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(3));

			//Package data
			targetAccExtra.AccExtraDriverLicense =
				inputProfileExtras.DriverLicense ?? targetAccExtra.AccExtraDriverLicense;
			targetAccExtra.AccExtraMobilityImpaired =
				(inputProfileExtras.MobilityImpaired == null) ?
					targetAccExtra.AccExtraMobilityImpaired :
					(inputProfileExtras.MobilityImpaired == 1) ?
						true :
						false;
			targetAccExtra.AccExtraHouseholdPosition =
				inputProfileExtras.HouseholdPosition ?? targetAccExtra.AccExtraHouseholdPosition;
			targetAccExtra.AccExtraNumberOfVehicles =
				inputProfileExtras.NoOfVehicles ?? targetAccExtra.AccExtraNumberOfVehicles;
			targetAccExtra.ModifiedAt = DateTime.Now;
			targetAccExtra.ModifiedBy = targetAcc.AccId.ToString();

			//Save changes to database
			_dbContext.AccountExtras.Update(targetAccExtra);
			_dbContext.SaveChanges();

			return Ok(true);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception401(0) + e.Message.ToString());
		}
	}
	[HttpGet, Route("GetFullProfileDetails/{id}"), Authorize]
	public List<ProfileAll>? GetFullProfileDetails(Guid id) {
		// To get the full profile from multiple tables

		List<ProfileAll> returnList = new List<ProfileAll>();

		var selectedProfile = (from a in _dbContext.Accounts
							   join ad in _dbContext.AccountDetails on a.AccId equals ad.AccId
							   join ae in _dbContext.AccountEmployments on a.AccId equals ae.AccId
							   join ax in _dbContext.AccountExtras on a.AccId equals ax.AccId
							   where a.AccId == id
							   select new {
								   accId = a.AccId,
								   username = a.AccUsername,
								   firstName = ad.AccDetailsFirstName,
								   lastName = ad.AccDetailsLastName,
								   gender = ad.AccDetailsGender,
								   age = ad.AccDetailsAge,
								   totalPoints = ad.AccDetailsTotalPoints,
								   phoneCountryCode = ad.AccDetailsPhoneCountryCode,
								   phoneNumber = ad.AccDetailsPhoneNumber,
								   country = ad.AccDetailsAddressCountry,
								   city = ad.AccDetailsAddressCity,
								   zipCode = ad.AccDetailsAddressZipCode,
								   streetAddress = ad.AccDetailsAddressStreet,
								   status = ae.AccEmpStatus,
								   occupation = ae.AccEmpOccupation,
								   employmentLocation = ae.AccEmpLocation,
								   startTime = ae.AccEmpStartTime,
								   endTime = ae.AccEmpEndTime,
								   annualSalary = ae.AccEmpAnnualSalary,
								   driverLicense = ax.AccExtraDriverLicense,
								   mobilityImpaired = ax.AccExtraMobilityImpaired,
								   householdPosition = ax.AccExtraHouseholdPosition,
								   noOfVehicles = ax.AccExtraNumberOfVehicles
							   }).ToList();

		foreach (var profile in selectedProfile) {
			ProfileAll profileAll = new ProfileAll();
			profileAll.accId = profile.accId;
			profileAll.Username = profile.username;
			profileAll.FirstName = profile.firstName;
			profileAll.LastName = profile.lastName;
			profileAll.Gender = profile.gender;
			profileAll.Age = profile.age;
			profileAll.TotalPoints = profile.totalPoints;
			profileAll.PhoneCountryCode = profile.phoneCountryCode;
			profileAll.PhoneNumber = profile.phoneNumber;
			profileAll.Country = profile.country;
			profileAll.City = profile.city;
			profileAll.ZipCode = profile.zipCode;
			profileAll.StreetAddress = profile.streetAddress;
			profileAll.Status = profile.status;
			profileAll.Occupation = profile.occupation;
			profileAll.EmploymentLocation = profile.employmentLocation;
			profileAll.StartTime = profile.startTime;
			profileAll.EndTime = profile.endTime;
			profileAll.AnnualSalary = profile.annualSalary;
			profileAll.DriverLicense = profile.driverLicense;
			profileAll.MobilityImpaired = profile.mobilityImpaired;
			profileAll.HouseholdPosition = profile.householdPosition;
			profileAll.NoOfVehicles = profile.noOfVehicles;

			returnList.Add(profileAll);
		};

		return returnList;
	}

	#endregion API Method(s)
}