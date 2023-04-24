using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class ResponseController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public ResponseController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	[HttpGet, Authorize]
	public List<Response> GetAll() {
		try {
			List<Response> allResponses = _dbContext.Responses.ToList();

			return allResponses;
		}
		catch {
			throw;
		}
	}
	[HttpGet, Route("{surveyId}"), Authorize]
	public Response? GetById(string surveyId) {
		try {
			Response? selectedResponse = _dbContext.Responses
				.Where(x => string.Equals(x.SurveyId.ToString(), surveyId))
				.FirstOrDefault();

			return selectedResponse;
		}
		catch {
			throw;
		}
	}

	[HttpPost, Authorize]
	public bool Create(
		[FromBody] Response response,
		string createdBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			response.CreatedBy = createdBy;
			response.CreatedAt = DateTime.Now;
			response.ModifiedBy = createdBy;
			response.ModifiedAt = DateTime.Now;

			_dbContext.Responses.Add(response);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPut, Authorize]
	public bool Update(
		[FromBody] Response response,
		string modifiedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			response.ModifiedBy = modifiedBy;
			response.ModifiedAt = DateTime.Now;

			_dbContext.Responses.Update(response);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpDelete, Route("{id}"), Authorize(Roles = "Admin")]
	public bool Delete(
		string id,
		string deletedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			Response? targetResponse = _dbContext.Responses
				.Where(x => string.Equals(x.SurveyId.ToString(), id))
				.FirstOrDefault();
			if (targetResponse == null) return false;

			//targetResponse.AccDisable = true;
			targetResponse.DeletedBy = deletedBy;
			targetResponse.DeletedAt = DateTime.Now;
			targetResponse.ModifiedBy = deletedBy;
			targetResponse.ModifiedAt = DateTime.Now;

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPost, Route("MobileResponse"), Authorize]
	public async Task<IActionResult> SubmitMobileResponse([FromBody] ResponseTemplate inputRes) {
		try {
			#region Account Id Retrieval
			//Check if token is a valid identity token
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Retrieve account Id from token
			Claim[] claims = identity.Claims.ToArray();
			string? accId = claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
				.Select(x => x.Value)
				.SingleOrDefault();

			//Check if token contains account Id
			if (string.IsNullOrEmpty(accId)) return StatusCode(401, new ErrorExceptionHelper().Exception401(1));

			//Try retrieve account
			Account? targetAccount = await _dbContext.Accounts
				.Where(x => string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();

			//Check account Id is valid
			if (targetAccount == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(2));
			#endregion Account Id Retrieval

			#region InputResponse Validation
			//Try retrieve survey
			Survey? targetSurvey = await _dbContext.Surveys
			.Where(x => string.Equals(inputRes.SurveyId, x.SurveyId.ToString()))
			.FirstOrDefaultAsync();

			//Check if survey Id is valid
			if (targetSurvey == null) return StatusCode(400, "0: Invalid survey id");

			//Check if response questions matches
			ResponseQuestionTemplate[] inputResQns = inputRes.ResQns ?? new ResponseQuestionTemplate[0];
			if (inputResQns.Length != targetSurvey.SurveyNoOfQns) return StatusCode(400, "Number of question response does not match survey");
			#endregion InputResponse Validation

			#region Data Packaging & Transformation
			DateTime now = DateTime.Now;
			string userId = targetAccount.AccId.ToString();
			Guid responseGuid = Guid.NewGuid();

			//Database response entry
			Response newEntry = new Response() {
				ResId = responseGuid,
				ResNoOfQns = targetSurvey.SurveyNoOfQns,
				ResponseDisable = false,

				ByPublicTransport = inputRes.isTravellingOnPublicTransport,

				CreatedAt = now,
				CreatedBy = userId,
				ModifiedAt = now,
				ModifiedBy = userId,

				SurveyId = targetSurvey.SurveyId,
				AccId = targetAccount.AccId
			};

			//Transform response questions
			foreach (ResponseQuestionTemplate currResQnsTemplate in inputResQns) {
				ResponseQuestion currResQns = new ResponseQuestion() {
					ResQnsId = Guid.NewGuid(),
					ResQnsType = currResQnsTemplate.SurveyQnsType,

					ResQnsString = currResQnsTemplate.SurveyQnsRes,
					ResQnsInt = (currResQnsTemplate.SurveyQnsType == 1) ? Int32.Parse(currResQnsTemplate.SurveyQnsRes) : null,
					ResQnsDecimal = null,

					CreatedAt = now,
					CreatedBy = userId,
					ModifiedAt = now,
					ModifiedBy = userId,

					ResId = responseGuid,
					QnsId = new Guid(currResQnsTemplate.SurveyQnsId)
				};

				newEntry.ResponseQuestions.Add(currResQns);
			}
			#endregion Data Packaging & Transformation

			#region Retrieve Target AccountTriggeredSurvey
			AccountTriggeredSurvey? dbAccTriggeredSurvey = await _dbContext.AccountTriggeredSurveys
				.Where(x =>
					x.Status == 0 &&
					x.AccId == targetAccount.AccId &&
					String.Equals(x.SurveyId.ToString(), inputRes.SurveyId))
				.FirstOrDefaultAsync();
			if (dbAccTriggeredSurvey == null)
				return StatusCode(500, "0:No AccountTriggeredSurvey found");

			dbAccTriggeredSurvey.Status = 1;
			#endregion Retrieve Target AccountTriggeredSurvey

			#region Response Points Transaction Creation
			//Acquire number of points of transaction
			int incomingPoints = targetSurvey.SurveyPoints;

			//Retrieve AccountDetails
			AccountDetail? targetAccDetails = await _dbContext.AccountDetails
				.Where(x => string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();
			//Check AccountDetails is valid
			if (targetAccDetails == null) return StatusCode(401, new ErrorExceptionHelper().Exception401(2));

			//Update net points of AccountDetails after tnx
			targetAccDetails.AccDetailsTotalPoints += incomingPoints;

			//Package new entry of AccountPointsTxn
			AccountPointsTxn incomingTxn = new AccountPointsTxn {
				AccPointsTxnId = Guid.NewGuid(),
				AccPointsTxnAmt = incomingPoints,

				CreatedAt = now,
				CreatedBy = userId,
				ModifiedAt = now,
				ModifiedBy = userId,

				AccId = targetAccount.AccId,
				Acc = targetAccount
			};
			#endregion Response Points Transaction Creation

			#region Update Database
			_dbContext.Responses.Add(newEntry);
			_dbContext.AccountPointsTxns.Add(incomingTxn);
			_dbContext.AccountDetails.Update(targetAccDetails);
			_dbContext.AccountTriggeredSurveys.Update(dbAccTriggeredSurvey);

			_dbContext.SaveChanges();
			#endregion Update Database

			return Ok();
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}
	#endregion API Method(s)
}