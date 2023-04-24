using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class SurveyTriggerController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public SurveyTriggerController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	[HttpGet, Authorize]
	public List<SurveyTrigger> GetAll() {
		try {
			List<SurveyTrigger> allSurveyTriggers = _dbContext.SurveyTriggers.Where(x => string.Equals(x.DeletedAt, null)).ToList();

			return allSurveyTriggers;
		}
		catch {
			throw;
		}
	}
	[HttpGet, Route("{surveyId}"), Authorize]
	public SurveyTrigger? GetById(string surveyId) {
		try {
			SurveyTrigger? selectedSurveyTrigger = _dbContext.SurveyTriggers
				.Where(x => string.Equals(x.TriggerId.ToString(), surveyId))
				.FirstOrDefault();

			return selectedSurveyTrigger;
		}
		catch {
			throw;
		}
	}

	[HttpPost, Authorize]
	public bool Create(
		[FromBody] SurveyTrigger surveyTrigger,
		string createdBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			surveyTrigger.CreatedBy = createdBy;
			surveyTrigger.CreatedAt = DateTime.Now;
			surveyTrigger.ModifiedBy = createdBy;
			surveyTrigger.ModifiedAt = DateTime.Now;

			_dbContext.SurveyTriggers.Add(surveyTrigger);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPut, Authorize]
	public bool Update(
		[FromBody] SurveyTrigger surveyTrigger,
		string modifiedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			surveyTrigger.ModifiedBy = modifiedBy;
			surveyTrigger.ModifiedAt = DateTime.Now;

			_dbContext.SurveyTriggers.Update(surveyTrigger);
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
			SurveyTrigger? targetSurveyTrigger = _dbContext.SurveyTriggers
				.Where(x => string.Equals(x.SurveyId.ToString(), id))
				.FirstOrDefault();
			if (targetSurveyTrigger == null) return false;

			targetSurveyTrigger.DeletedBy = deletedBy;
			targetSurveyTrigger.DeletedAt = DateTime.Now;
			targetSurveyTrigger.ModifiedBy = deletedBy;
			targetSurveyTrigger.ModifiedAt = DateTime.Now;

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpGet, Route("CheckTriggers"), AllowAnonymous]
	public async Task<IActionResult> CheckTriggers() {
		#region Runtime Variable(s) Initialization
		DateTime lastChecked = DateTime.Now.AddYears(-2);

		//Stores transformed trigger conditions
		Dictionary<string, SurveyTriggerSet> triggerDict = new Dictionary<string, SurveyTriggerSet>();
		//Stores info of which survey is triggered for which user
		Dictionary<string, List<string[]>> triggeredDict = new Dictionary<string, List<string[]>>();

		string[] activeSurveys;
		SurveyTrigger[] surveyTriggers;
		GpsLog[] uncheckedLogs;
		#endregion Runtime Variable(s) Initialization

		#region Expiry Checking
		try {
			#region Expiry Checking - Section Runtime Variable(s) Initialization
			AccountTriggeredSurvey[] expiredTriggeredSurveys;
			#endregion Expiry Checking - Section Runtime Variable(s) Initialization

			#region Expiry Checking - Update Expired
			// 0: Available/Pending
			// 1: Completed
			// 2: Expired

			expiredTriggeredSurveys = await _dbContext.AccountTriggeredSurveys
				.Where(x => x.Status == 0 && x.ExpireBy <= DateTime.Now)
				.ToArrayAsync();

			foreach (AccountTriggeredSurvey expiredTriggeredSurvey in expiredTriggeredSurveys)
				expiredTriggeredSurvey.Status = 2;
			#endregion Expiry Checking - Update Expired

			#region Expiry Checking - Update Database
			_dbContext.SaveChanges();
			#endregion Expiry Checking - Update Database
		}
		catch (Exception e) {
			//Encountered exception while checking expiry
			return StatusCode(500, "Error: EC001");
		}
		#endregion Expiry Checking

		#region Data Retrieval
		try {
			#region Data Retrieval - Survey Triggers
			activeSurveys = await _dbContext.Surveys
				.Where(x => !x.SurveyDisable)
				.Select(x => x.SurveyId.ToString())
				.ToArrayAsync();

			surveyTriggers = await _dbContext.SurveyTriggers
				.Where(x => activeSurveys.Contains(x.SurveyId.ToString()))
				.ToArrayAsync();
			#endregion Data Retrieval - Survey Triggers

			#region Data Retrieval - GPS Logs
			uncheckedLogs = await _dbContext.GpsLogs
				.Where(x => x.GpsTimestamp >= lastChecked)
				.ToArrayAsync();
			#endregion Data Retrieval - GPS Logs
		}
		catch {
			//Encountered exception while data retrieving
			return StatusCode(500, "Error: DR001");
		}
		#endregion Data Retrieval

		#region Data Processing
		try {
			#region Data Processing - Section Runtime Variable(s) Initialization
			string currentSurveyId, currentAccId;
			double currentLong, currentLat;

			SurveyTriggerData surveyTriggerData;
			#endregion Data Processing - Section Runtime Variable(s) Initialization

			#region Data Processing - Survey Triggers Transformation
			foreach (SurveyTrigger surveyTrigger in surveyTriggers) {
				currentSurveyId = surveyTrigger.SurveyId.ToString() ?? "";
				string[] dataParts = (surveyTrigger.LatLong ?? "0,0").Split(',');

				//For each set of coordinates
				for (int i = 0; i < dataParts.Length / 2; i++) {
					try {
						if (!triggerDict.ContainsKey(currentSurveyId) &&
							!String.IsNullOrEmpty(currentSurveyId)) {
							triggerDict.Add(
								currentSurveyId, new SurveyTriggerSet(
									double.Parse(dataParts[i * 2].Trim()),
									double.Parse(dataParts[(i * 2) + 1].Trim()),
									surveyTrigger.TriggerRadius ?? 10,
									surveyTrigger.SurveyId,
									surveyTrigger.TriggerId
								)
							);
						}
						else if (triggerDict.ContainsKey(currentSurveyId)) {
							SurveyTriggerSet triggerData = triggerDict[currentSurveyId];

							triggerData.AddTrigger(
								double.Parse(dataParts[i * 2].Trim()),
								double.Parse(dataParts[(i * 2) + 1].Trim()),
								surveyTrigger.TriggerRadius ?? 10,
								surveyTrigger.SurveyId,
								surveyTrigger.TriggerId
							);

							triggerDict[currentSurveyId] = triggerData;
						}
					}
					catch (Exception) {
						//Exclude survey trigger
					}
				}
			}
			#endregion Data Processing - Survey Triggers Transformation

			#region Data Processing - Check Survey Triggers
			foreach (GpsLog uncheckedLog in uncheckedLogs) {
				if (uncheckedLog.AccId == null) continue;

				currentAccId = uncheckedLog.AccId.ToString() ?? "";

				foreach (KeyValuePair<string, SurveyTriggerSet> trigger in triggerDict) {
					//Check if current survey id is already triggered for current account id
					bool userExists = triggeredDict.ContainsKey(currentAccId);
					if (userExists) {
						List<string[]> x = triggeredDict[currentAccId];
						if (String.Equals(x[0], trigger.Key)) {
							continue;
						}
					}

					//Convert current long lat to double
					currentLong = Decimal.ToDouble(uncheckedLog.Longitude);
					currentLat = Decimal.ToDouble(uncheckedLog.Latitude);

					//If trigger conditions applied
					surveyTriggerData = trigger.Value.IsTriggered(currentLong, currentLat);
					if (surveyTriggerData.IsTriggered) {
						//If current account id is not yet tracked in dictionary
						if (!triggeredDict.ContainsKey(currentAccId)) {
							triggeredDict.Add(currentAccId, new List<string[]> {
								new string[]
								{
									await GetTrip_ConcatString(uncheckedLog.GpsId.ToString()), //Triggered Trip GpsLogIds
									surveyTriggerData.TriggeredSurveyId.ToString(), //SurveyId
									surveyTriggerData.TriggeredSurveyTriggerId.ToString(), //SurveyTriggerId
									uncheckedLog.GpsId.ToString() //Triggered GpsLogId
								}
							});
						}
						//If current account id being tracked in dictionary
						else {
							triggeredDict[currentAccId].Add(
								new string[] {
									await GetTrip_ConcatString(uncheckedLog.GpsId.ToString()), //Triggered Trip GpsLogIds
									surveyTriggerData.TriggeredSurveyId.ToString(), //SurveyId
									surveyTriggerData.TriggeredSurveyTriggerId.ToString(), //SurveyTriggerId
									uncheckedLog.GpsId.ToString() //Triggered GpsLogId
								}
							);
						}
					}
				}
			}
			#endregion Data Processing - Check Survey Triggers
		}
		catch {
			//Encountered exception while processing
			return StatusCode(500, "Error: DP001");
		}
		#endregion Data Processing

		#region Database Updating
		try {
			#region Database Updating - Section Runtime Variable(s) Initialization
			List<AccountTriggeredSurvey> allTriggereds;
			List<AccountTriggeredSurvey> ongoingTriggereds;
			List<AccountTriggeredSurvey> completedTriggereds;
			#endregion Database Updating - Section Runtime Variable(s) Initialization

			#region Database Updating - Retrieve All Triggereds
			allTriggereds = await _dbContext.AccountTriggeredSurveys
				.ToListAsync();
			#endregion Database Updating - Retrieve All Triggereds

			#region Database Updating - Filter Triggereds To Different Types
			ongoingTriggereds = allTriggereds.Where(x => x.Status == 0).ToList();
			completedTriggereds = allTriggereds.Where(x => x.Status == 1).ToList();
			#endregion Database Updating - Filter Triggereds To Different Types

			#region Database Updating - Update Triggered Surveys
			foreach (KeyValuePair<string, List<string[]>> triggered in triggeredDict) {
				AccountTriggeredSurvey? existingTriggereds = ongoingTriggereds
					.Where(x =>
						String.Equals(x.AccId.ToString(), triggered.Key) &&
						String.Equals(x.SurveyId.ToString(), triggered.Value[0].ToString())
					)
					.FirstOrDefault();

				AccountTriggeredSurvey? onCooldown = completedTriggereds
					.Where(x =>
						String.Equals(x.AccId.ToString(), triggered.Key) &&
						String.Equals(x.SurveyId.ToString(), triggered.Value[0].ToString()) &&
						x.ModifiedAt >= DateTime.Now.AddMonths(-6)
					)
					.FirstOrDefault();

				if (existingTriggereds != null || onCooldown != null) continue;

				DateTime timeStamp = DateTime.Now;
				AccountTriggeredSurvey incoming = new AccountTriggeredSurvey {
					AccTriggerId = Guid.NewGuid(),

					GpsLogIds = triggered.Value[0][0].ToString(),
					Status = 0,

					SurveyId = Guid.Parse(triggered.Value[0][1].ToString() ?? ""),
					AccId = Guid.Parse(triggered.Key),
					TriggerId = Guid.Parse(triggered.Value[0][2].ToString() ?? ""),
					GpsId = Guid.Parse(triggered.Value[0][3].ToString() ?? ""),

					ExpireBy = timeStamp.AddDays(2),

					CreatedBy = null,
					CreatedAt = timeStamp,
					ModifiedBy = null,
					ModifiedAt = timeStamp
				};
				_dbContext.AccountTriggeredSurveys.Add(incoming);
			}
			#endregion Database Updating - Update Triggered Surveys

			#region Database Updating - Update lastChecked
			_dbContext.SurveyTriggerLogs.Add(new SurveyTriggerLog {
				SurveyTriggerLogId = Guid.NewGuid(),
				LogDateTime = DateTime.Now
			});
			#endregion Database Updating - Update lastChecked

			_dbContext.SaveChanges();
		}
		catch (Exception e) {
			//Encountered exception while updating
			return StatusCode(500, "Error: DU001");
		}
		#endregion Database Updating

		return Ok();
	}

	[HttpGet, Route("GetTrip"), AllowAnonymous]
	public async Task<IActionResult> IGetTrip(string targetGpsLog) {
		//This method is an API wrapper of the logic function to allow unit testing through API

		GpsLog[] result;

		try {
			result = await GetTrip(targetGpsLog);
		}
		catch {
			return StatusCode(500);
		}

		return Ok(result);
	}
	private async Task<string> GetTrip_ConcatString(string targetGpsLog) {
		GpsLog[] result;
		string returnData = "";

		try {
			result = await GetTrip(targetGpsLog);

			for (int i = 0; i < result.Length; i++) {
				returnData += result[i].GpsId.ToString();

				if (i < result.Length - 1) returnData += ",";
			}
		}
		catch {
			throw;
		}

		return returnData;
	}
	private async Task<GpsLog[]> GetTrip(string targetGpsLog) {
		#region Runtime Variable(s) Initialization
		GpsLog dbTargetGpsLog, neighbouringGpsLog;
		List<GpsLog> returnData = new List<GpsLog>();
		GpsLog[] previousLogs, upcomingLogs;
		#endregion Runtime Variable(s) Initialization

		#region Data Retrieval
		GpsLog? temp = await _dbContext.GpsLogs
			.Where(x => String.Equals(x.GpsId.ToString(), targetGpsLog))
			.FirstOrDefaultAsync();
		if (temp == null) throw new Exception();
		else dbTargetGpsLog = temp;

		//Retrieve maximum of 100 past logs
		previousLogs = await _dbContext.GpsLogs
			.Where(x =>
				x.CreatedBy == dbTargetGpsLog.CreatedBy &&
				x.GpsTimestamp < dbTargetGpsLog.GpsTimestamp)
			.OrderByDescending(x => x.GpsTimestamp)
			.Take(100)
			.ToArrayAsync();

		//Retrieve maximum of 100 future logs
		upcomingLogs = await _dbContext.GpsLogs
			.Where(x =>
				x.CreatedBy == dbTargetGpsLog.CreatedBy &&
				x.GpsTimestamp > dbTargetGpsLog.GpsTimestamp)
			.OrderBy(x => x.GpsTimestamp)
			.Take(100)
			.ToArrayAsync();
		#endregion Data Retrieval

		#region Processing - Trip Backtracking
		neighbouringGpsLog = dbTargetGpsLog;

		foreach (GpsLog previousLog in previousLogs) {
			if (IsTravelling(previousLog, neighbouringGpsLog)) {
				returnData.Add(previousLog);
				neighbouringGpsLog = previousLog;
			}
		}
		#endregion Processing - Trip Backtracking

		#region Processing - Trip Pivot Construction
		returnData.Reverse();
		returnData.Add(dbTargetGpsLog);
		#endregion Processing - Trip Pivot Construction

		#region Processing - Trip Tracking
		neighbouringGpsLog = dbTargetGpsLog;

		foreach (GpsLog upcomingLog in upcomingLogs) {
			if (IsTravelling(upcomingLog, neighbouringGpsLog)) {
				returnData.Add(upcomingLog);
				neighbouringGpsLog = upcomingLog;
			}
		}
		#endregion Processing - Trip Tracking

		return returnData.ToArray();
	}

	[HttpGet, Route("IsTravelling"), AllowAnonymous]
	public async Task<IActionResult> IIsTravelling(string gpsId01, string gpsId02) {
		//This method is an API wrapper of the logic function to allow unit testing through API

		GpsLog gpsLog01, gpsLog02;
		GpsLog? temp = null;

		temp = await _dbContext.GpsLogs
			.Where(x => String.Equals(x.GpsId.ToString(), gpsId01))
			.FirstOrDefaultAsync();
		if (temp == null) return StatusCode(400);
		else gpsLog01 = temp;

		temp = await _dbContext.GpsLogs
			.Where(x => String.Equals(x.GpsId.ToString(), gpsId02))
			.FirstOrDefaultAsync();
		if (temp == null) return StatusCode(400);
		else gpsLog02 = temp;

		try {
			bool result = IsTravelling(gpsLog01, gpsLog02);

			return Ok(result);
		}
		catch {
			return StatusCode(500);
		}
	}
	private bool IsTravelling(GpsLog gpsLog01, GpsLog gpsLog02) {
		//Function to determine if travelling is occuring between 2 GpsLogs
		//Utilizes conditional voting, conditions are scalable in the future

		try {
			#region Runtime Variable(s) Initialization
			//Runtime variable(s) initialization
			HelperService helper = new HelperService();
			int trueVotes = 0, falseVotes = 0;

			//Vote-killing conditions initilization
			int maxTimeElapsed_sec = 600;

			//Voting conditions initilization
			double minDistDiff_km = 0.01;
			double minAvgSpeed_mPerSec = 0.003;

			//Vote-killing conditions calculations
			double secElapsed = 0.0;

			//Voting conditions calculations
			double distDiff = 0.0;
			double avgSpeed = 0.0;
			#endregion Runtime Variable(s) Initialization

			#region Vote-killer Checkings
			//Time Elapsed
			secElapsed = (gpsLog01.GpsTimestamp - gpsLog02.GpsTimestamp).TotalSeconds;
			secElapsed = (secElapsed < 0) ? secElapsed * -1 : secElapsed;
			if (secElapsed > Convert.ToDouble(maxTimeElapsed_sec)) return false;
			#endregion Vote-killer Checkings

			#region Conditional Voting
			//Voting - Distance Difference
			distDiff = helper.CalculateDistBtwCoords(
				(double)gpsLog01.Latitude,
				(double)gpsLog01.Longitude,
				(double)gpsLog02.Latitude,
				(double)gpsLog02.Longitude
			);
			if (distDiff < minDistDiff_km) falseVotes++;
			else trueVotes++;

			//Voting - Average Speed
			avgSpeed = (distDiff * 1000) / secElapsed;
			if (avgSpeed < minAvgSpeed_mPerSec) falseVotes++;
			else trueVotes++;
			#endregion Conditional Voting

			return trueVotes > falseVotes;
		}
		catch {
			throw;
		}
	}
	#endregion API Method(s)
}