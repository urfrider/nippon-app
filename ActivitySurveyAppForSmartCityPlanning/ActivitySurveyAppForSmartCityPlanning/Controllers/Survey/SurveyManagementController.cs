using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class SurveyManagementController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public SurveyManagementController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	/*  -	Question Types Legend 	-
		Type 0 = Open Ended
		Type 1 = Radio Button (1 Answer only)
		Type 2 = Multiple Choice (Multiple Answer)
	*/
	[HttpPost, Route("CreateSurvey"), Authorize(Roles = "DashboardUser")]
	public async Task<IActionResult> CreateSurvey([FromBody] SurveyTemplate inputSurvey) {
		try {
			#region Session Retrieval
			//Check if token is a valid identity token
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(400, new ErrorExceptionHelper().Exception401(0));

			//Retrieve account Id from token
			Claim[] claims = identity.Claims.ToArray();
			string? accId = claims
				.Where(x => x.Type == ClaimTypes.NameIdentifier)
				.Select(x => x.Value)
				.SingleOrDefault();

			//Check if token contains account Id
			if (string.IsNullOrEmpty(accId))
				return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Try retrieve account
			Account? targetAccount = await _dbContext.Accounts
				.Where(x => string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();

			//Check account Id is valid
			if (targetAccount == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(2));
			#endregion Session Retrieval

			#region Survey Data Transformation
			//Validate survey json data
			if (string.IsNullOrEmpty(inputSurvey.Title))
				return StatusCode(400, "0:Invalid survey data; No survey title found");
			if (inputSurvey.Questions == null)
				return StatusCode(400, "1:Invalid survey data; No survey questions found");

			//Generate Guid for survey
			Guid surveyId = Guid.NewGuid();
			DateTime dateTime = DateTime.Now;

			//Transform survey to model object
			Survey survey = new Survey() {
				SurveyPoints = inputSurvey.Points,
				SurveyTitle = inputSurvey.Title,
				SurveyDesc = inputSurvey.Description ?? "",
				SurveyId = surveyId,
				SurveyDisable = false
			};

			//Dissect survey questions
			int noOfQuestions = 0;
			foreach (SurveyQuestionTemplate inputSurveyQuestion in inputSurvey.Questions) {
				//Acquire question options count
				string[] options = inputSurveyQuestion.Options ?? Array.Empty<string>();

				//Validate survey question json data
				if (string.IsNullOrEmpty(inputSurvey.Title))
					return StatusCode(400, "2:Invalid survey data; No question found in question");
				if (options.Length < 1 && inputSurveyQuestion.Type > 0)
					return StatusCode(400, "3:Invalid survey data; No question options found in question");

				//Transform survey questions to model object
				SurveyQuestion surveyQuestion = new SurveyQuestion() {
					SurveyId = surveyId,
					QnsId = Guid.NewGuid(),
					QnsOrder = (noOfQuestions + 1),
					Qns = inputSurveyQuestion.Question ?? "**IMPOSSIBLE_CASE**",
					QnsType = inputSurveyQuestion.Type,

					QnsOption01 = (options.Length >= 1) ? options[0] : null,
					QnsOption02 = (options.Length >= 2) ? options[1] : null,
					QnsOption03 = (options.Length >= 3) ? options[2] : null,
					QnsOption04 = (options.Length >= 4) ? options[3] : null,
					QnsOption05 = (options.Length >= 5) ? options[4] : null,
					QnsOption06 = (options.Length >= 6) ? options[5] : null,
					QnsOption07 = (options.Length >= 7) ? options[6] : null,
					QnsOption08 = (options.Length >= 8) ? options[7] : null,

					CreatedAt = dateTime,
					CreatedBy = targetAccount.AccId.ToString(),
					ModifiedAt = dateTime,
					ModifiedBy = targetAccount.AccId.ToString()
				};

				survey.SurveyQuestions.Add(surveyQuestion);
				noOfQuestions++;
			}

			float inputLat = float.Parse(inputSurvey.Latitude ?? "0.0");
			float inputLong = float.Parse(inputSurvey.Longitude ?? "0.0");

			//Survey trigger info
			SurveyTrigger surveyTrigger = new SurveyTrigger() {
				TriggerId = Guid.NewGuid(),
				LatLong = inputLat.ToString() + "," + inputLong.ToString(),

				TriggerRadius = inputSurvey.Radius,
				TriggerCooldown = inputSurvey.Cooldown,

				CreatedAt = dateTime,
				CreatedBy = targetAccount.AccId.ToString(),
				ModifiedAt = dateTime,
				ModifiedBy = targetAccount.AccId.ToString(),

				SurveyId = surveyId
			};

			survey.SurveyTriggers.Add(surveyTrigger);

			//Append metadata to survey model object
			survey.SurveyNoOfQns = noOfQuestions;
			survey.CreatedAt = dateTime;
			survey.CreatedBy = targetAccount.AccId.ToString();
			survey.ModifiedAt = dateTime;
			survey.ModifiedBy = targetAccount.AccId.ToString();
			#endregion Survey Data Transformation

			#region Saving Survey Data to Database
			_dbContext.Surveys.Add(survey);
			await _dbContext.SaveChangesAsync();
			#endregion Saving Survey Data to Database

			return Ok("Survey Successfully Created.");
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	[HttpGet, Route("GetTriggeredSurvey/{surveyId}"), Authorize]
	public async Task<IActionResult> GetTriggeredSurvey(string surveyId) {
		try {
			#region Session Retrieval
			//Check if token is a valid identity token
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(400, "0: Invalid identity token");

			//Retrieve account Id from token
			Claim[] claims = identity.Claims.ToArray();
			string? accId = claims
				.Where(x => x.Type == ClaimTypes.NameIdentifier)
				.Select(x => x.Value)
				.SingleOrDefault();

			//Check if token contains account Id
			if (string.IsNullOrEmpty(accId))
				return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Try retrieve account
			Account? targetAccount = await _dbContext.Accounts
				.Where(x => string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();

			//Check account Id is valid
			if (targetAccount == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(2));
			#endregion Session Retrieval

			#region Survey Data Retrieval
			//Retrieve survey
			Survey? dbSurvey = await _dbContext.Surveys
				.Where(x => x.SurveyId.ToString().Equals(surveyId))
				.FirstOrDefaultAsync();
			if (dbSurvey == null)
				return StatusCode(400, "1: Invalid survey Id");
			Survey survey = dbSurvey;

			//Retrieve survey triggered record
			AccountTriggeredSurvey? dbAccTriggeredSurvey = await _dbContext.AccountTriggeredSurveys
				.Where(x =>
					x.Status == 0 &&
					x.AccId == targetAccount.AccId &&
					x.SurveyId == survey.SurveyId)
				.FirstOrDefaultAsync();
			if (dbAccTriggeredSurvey == null)
				return StatusCode(500, "0: No AccountTriggeredSurvey found");
			AccountTriggeredSurvey accTriggeredSurvey = dbAccTriggeredSurvey;

			//Retrieve survey questions
			List<SurveyQuestion> surveyQuestions = await _dbContext.SurveyQuestions
				.Where(x => x.SurveyId.Equals(survey.SurveyId))
				.ToListAsync();
			surveyQuestions = surveyQuestions
				.OrderBy(x => x.QnsOrder)
				.ToList();

			//Extract tripGpsLogs' Ids
			if (accTriggeredSurvey.GpsLogIds.IsNullOrEmpty())
				return StatusCode(500, "1: No trip found");
			string[] tripIds = (accTriggeredSurvey.GpsLogIds ?? "").Split(",");

			//Retrieve triggeredGpsLog
			GpsLog? dbTrigger = await _dbContext.GpsLogs
				.Where(x => x.GpsId == accTriggeredSurvey.GpsId)
				.FirstOrDefaultAsync();
			if (dbTrigger == null)
				return StatusCode(500, "2: No trigger GPS log found");
			decimal[] triggerLongLat = new decimal[] { dbTrigger.Latitude, dbTrigger.Longitude };

			//Retrieve tripGpsLogs longlats
			decimal[][] tripGpsLogs = await _dbContext.GpsLogs
				.Where(x => tripIds.Contains(x.GpsId.ToString()))
				.Select(x => new decimal[] { x.Latitude, x.Longitude })
				.ToArrayAsync();

			int isTravelling = (tripGpsLogs.Length >= 3) ? 1 : 0;
			int isTravellingOnPublicTransport =
				((await ByPublicTransport(tripIds)) && isTravelling == 1) ? 1 : 0;

			int triggeredPos = 0;
			for (int i = 0; i < tripGpsLogs.Length; i++) {
				if (Enumerable.SequenceEqual(tripGpsLogs[i], triggerLongLat)) {
					triggeredPos = i;
					break;
				}
			}
			#endregion Survey Data Retrieval

			#region Survey Data Transformation
			//Package survey questions
			List<SurveyQuestionTemplate> returnSurveyQuestion = new List<SurveyQuestionTemplate>();
			foreach (SurveyQuestion surveyQuestion in surveyQuestions) {
				//Package question options
				List<string> options = new List<string>{
					surveyQuestion.QnsOption01 ?? "",
					surveyQuestion.QnsOption02 ?? "",
					surveyQuestion.QnsOption03 ?? "",
					surveyQuestion.QnsOption04 ?? "",
					surveyQuestion.QnsOption05 ?? "",
					surveyQuestion.QnsOption06 ?? "",
					surveyQuestion.QnsOption07 ?? "",
					surveyQuestion.QnsOption08 ?? ""
				};
				options.RemoveAll(x => string.IsNullOrEmpty(x));

				//Package question
				SurveyQuestionTemplate currentQuestion = new SurveyQuestionTemplate();
				currentQuestion.QuestionId = surveyQuestion.QnsId.ToString();
				currentQuestion.Question = surveyQuestion.Qns;
				currentQuestion.Type = surveyQuestion.QnsType;
				currentQuestion.Options = options.ToArray();

				//Append question to survey's question list
				returnSurveyQuestion.Add(currentQuestion);
			}

			//Package survey data
			SurveyTemplate returnSurvey = new SurveyTemplate {
				Id = surveyId.ToString(),

				Title = survey.SurveyTitle,
				Description = survey.SurveyDesc,
				Points = survey.SurveyPoints,

				IsTravelling = isTravelling,
				IsTravellingOnPublicTransport = isTravellingOnPublicTransport,
				TripLatLongs = tripGpsLogs,
				TriggeredLatLong = triggerLongLat,
				TriggeredPos = triggeredPos,

				Questions = returnSurveyQuestion.ToArray()
			};

			#endregion Survey Data Transformation

			return Ok(returnSurvey);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	[HttpGet, Route("GetSurvey/{surveyId}"), Authorize]
	public async Task<IActionResult> GetSurvey(string surveyId) {
		try {
			#region Session Retrieval
			//Check if token is a valid identity token
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(400, "0:Invalid identity token");

			//Retrieve account Id from token
			Claim[] claims = identity.Claims.ToArray();
			string? accId = claims
				.Where(x => x.Type == ClaimTypes.NameIdentifier)
				.Select(x => x.Value)
				.SingleOrDefault();

			//Check if token contains account Id
			if (string.IsNullOrEmpty(accId))
				return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Try retrieve account
			Account? targetAccount = await _dbContext.Accounts
				.Where(x => string.Equals(x.AccId.ToString(), accId))
				.FirstOrDefaultAsync();

			//Check account Id is valid
			if (targetAccount == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(2));
			#endregion Session Retrieval

			#region Survey Data Retrieval
			Survey? dbSurvey = await _dbContext.Surveys
				.Where(x => x.SurveyId.ToString().Equals(surveyId))
				.FirstOrDefaultAsync();

			if (dbSurvey == null)
				return StatusCode(400, "0:Invalid survey Id");
			Survey survey = dbSurvey;

			List<SurveyQuestion> surveyQuestions = await _dbContext.SurveyQuestions
				.Where(x => x.SurveyId.Equals(survey.SurveyId))
				.ToListAsync();
			surveyQuestions = surveyQuestions
				.OrderBy(x => x.QnsOrder)
				.ToList();
			#endregion Survey Data Retrieval

			#region Survey Data Transformation
			//Package survey questions
			List<SurveyQuestionTemplate> returnSurveyQuestion = new List<SurveyQuestionTemplate>();
			foreach (SurveyQuestion surveyQuestion in surveyQuestions) {
				//Package question options
				List<string> options = new List<string>{
					surveyQuestion.QnsOption01 ?? "",
					surveyQuestion.QnsOption02 ?? "",
					surveyQuestion.QnsOption03 ?? "",
					surveyQuestion.QnsOption04 ?? "",
					surveyQuestion.QnsOption05 ?? "",
					surveyQuestion.QnsOption06 ?? "",
					surveyQuestion.QnsOption07 ?? "",
					surveyQuestion.QnsOption08 ?? ""
				};
				options.RemoveAll(x => string.IsNullOrEmpty(x));

				//Package question
				SurveyQuestionTemplate currentQuestion = new SurveyQuestionTemplate();
				currentQuestion.QuestionId = surveyQuestion.QnsId.ToString();
				currentQuestion.Question = surveyQuestion.Qns;
				currentQuestion.Type = surveyQuestion.QnsType;
				currentQuestion.Options = options.ToArray();

				//Append question to survey's question list
				returnSurveyQuestion.Add(currentQuestion);
			}

			//Package survey
			SurveyTemplate returnSurvey = new SurveyTemplate {
				Id = surveyId.ToString(),
				Title = survey.SurveyTitle,
				Points = survey.SurveyPoints,
				Description = survey.SurveyDesc,
				Questions = returnSurveyQuestion.ToArray()
			};

			#endregion Survey Data Transformation

			return Ok(returnSurvey);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}

	// Customized response for Survey Retrieval
	[HttpGet, Route("GetSurvey"), Authorize]
	public async Task<IActionResult> GetTriggeredSurveys() {
		#region Session Retrieval
		//Check if token is a valid identity token
		ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
		if (identity == null)
			return StatusCode(400, "0:Invalid identity token");

		//Retrieve account Id from token
		Claim[] claims = identity.Claims.ToArray();
		string? accId = claims
			.Where(x => x.Type == ClaimTypes.NameIdentifier)
			.Select(x => x.Value)
			.SingleOrDefault();

		//Check if token contains account Id
		if (string.IsNullOrEmpty(accId))
			return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

		//Try retrieve account
		Account? targetAccount = await _dbContext.Accounts
			.Where(x => string.Equals(x.AccId.ToString(), accId))
			.FirstOrDefaultAsync();

		//Check account Id is valid
		if (targetAccount == null)
			return StatusCode(401, new ErrorExceptionHelper().Exception401(2));
		#endregion Session Retrieval

		#region Runtime Variable(s) Initialization
		AccountTriggeredSurvey[] triggeredSurveys;
		string[] targetSurveyGuids;
		Survey[] targetSurveys;
		List<SurveyCheckTrigger> transformedTriggeredSurveys;
		SurveyCheckTrigger[] returnData;
		#endregion Runtime Variable(s) Initialization

		#region Data Retrieval
		try {
			triggeredSurveys = await _dbContext.AccountTriggeredSurveys
				.Where(x =>
					x.AccId == targetAccount.AccId &&
					x.Status == 0
				)
				.ToArrayAsync();

			targetSurveyGuids = triggeredSurveys
				.Where(x => x.SurveyId != null)
				.Select(x => x.SurveyId.ToString() ?? "")
				.ToArray();

			targetSurveys = await _dbContext.Surveys
				.Where(x => targetSurveyGuids.Contains(x.SurveyId.ToString()))
				.ToArrayAsync();
		}
		catch {
			return StatusCode(500);
		}
		#endregion Data Retrieval

		#region Data Transformation
		try {
			transformedTriggeredSurveys = new List<SurveyCheckTrigger>();
			foreach (AccountTriggeredSurvey triggeredSurvey in triggeredSurveys) {
				Survey? targetSurvey = targetSurveys
					.Where(y => y.SurveyId == triggeredSurvey.SurveyId)
					.FirstOrDefault();
				if (targetSurvey != null && triggeredSurvey.SurveyId != null) {
					transformedTriggeredSurveys.Add(
						new SurveyCheckTrigger {
							SurveyId = triggeredSurvey.SurveyId ?? Guid.Empty,
							SurveyNoOfQns = targetSurvey.SurveyNoOfQns,
							SurveyTitle = targetSurvey.SurveyTitle,
							SurveyDisable = targetSurvey.SurveyDisable,
							SurveyPoints = targetSurvey.SurveyPoints
						}
					);
				}
			}
			returnData = transformedTriggeredSurveys.ToArray();
		}
		catch {
			return StatusCode(500);
		}
		#endregion Data Transformation

		return Ok(returnData);
	}

	[HttpGet, Route("ByPublicTransport"), Authorize]
	public async Task<IActionResult> IByPublicTransport(string[] gpsLogId) {
		//This method is an API wrapper of the logic function to allow unit testing through API

		bool result = false;

		try {
			result = await ByPublicTransport(gpsLogId);
		}
		catch {
			return StatusCode(500);
		}

		return Ok(result);
	}
	private async Task<bool> ByPublicTransport(string[] gpsLogId) {
		#region Runtime Variable(s) Initialization
		//Algorithm setting(s) instantiation
		double targetDist = 5.0, qualifyingMargin = 0.2;

		//Runtime variable(s) instantiation
		HelperService helper = new HelperService();
		List<string> qualifiedGpsLogs = new List<string>();
		double distBtwCoords = 0.0;
		bool returnVal = false;

		//Database variable(s) instantiation
		List<GpsLog> currentGpsLogs = new List<GpsLog>();
		List<PublicTransport> allPublicTransportLocation = new List<PublicTransport>();
		#endregion Runtime Variable(s) Initialization

		#region Data Retrieval
		try {
			//Retrieve target GpsLogs from database
			currentGpsLogs = await _dbContext.GpsLogs
				.Where(x => gpsLogId.Contains(x.GpsId.ToString()))
				.ToListAsync();

			//Retrieve all bus stop locations from database
			allPublicTransportLocation = await _dbContext.PublicTransports
				.ToListAsync();
		}
		catch {
			//Encountered exception while data retrieving
			throw;
		}
		#endregion Data Retrieval

		#region Data Processing
		try {
			//Check if target GpsLogs are within proximity of public transport services
			foreach (GpsLog gpslog in currentGpsLogs) {
				foreach (PublicTransport publicTransport in allPublicTransportLocation) {
					distBtwCoords = helper.CalculateDistBtwCoords(
						(double)gpslog.Latitude,
						(double)gpslog.Longitude,
						Decimal.ToDouble(publicTransport.TransportLatitude ?? 0),
						Decimal.ToDouble(publicTransport.TransportLongitude ?? 0)
					);

					if (distBtwCoords <= targetDist) qualifiedGpsLogs.Add(gpslog.GpsId.ToString());
				}
			}

			//Check if passes qualifying margin
			double margin = qualifiedGpsLogs.Count / gpsLogId.Length;
			returnVal = margin >= qualifyingMargin;
		}
		catch {
			//Encountered exception while data processing
			throw;
		}
		#endregion Data Processing

		return returnVal;
	}
	#endregion API Method(s)
}