using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers.GPS;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class GpsCoordinatesController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public GpsCoordinatesController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	[HttpPost, Route("SendGPSList"), Authorize]
	public async Task<IActionResult> SendCoordinates([FromBody] List<GpsLogInput> gpsLogInput) {
		DateTime now = DateTime.Now;

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

			//Transform incoming GpsLogs
			List<GpsLog> newEntries = new List<GpsLog>();
			decimal tLatitude, tLongitude, tAccuracy, tAltitude, tAltitudeAccuracy, tHeading, tSpeed;
			foreach (GpsLogInput gpsLog in gpsLogInput) {
				//Log datetime now for tracking
				now = DateTime.Now;

				//Values validation & truncation
				tLatitude = Math.Round(gpsLog.Latitude, 7);
				tLatitude = (tLatitude > 999) ? 999 + (tLatitude - Math.Truncate(tLatitude)) : tLatitude;
				tLongitude = Math.Round(gpsLog.Longitude, 7);
				tLongitude = (tLongitude > 999) ? 999 + (tLongitude - Math.Truncate(tLongitude)) : tLongitude;
				tAccuracy = Math.Round(gpsLog.Accuracy ?? 0.0M, 15);
				tAccuracy = (tAccuracy > 99999) ? 99999 + (tAccuracy - Math.Truncate(tAccuracy)) : tAccuracy;
				tAltitude = Math.Round(gpsLog.Altitude ?? 0.0M, 15);
				tAltitude = (tAltitude > 999999) ? 999999 + (tAltitude - Math.Truncate(tAltitude)) : tAltitude;
				tAltitudeAccuracy = Math.Round(gpsLog.AltitudeAccuracy ?? 0.0M, 15);
				tAltitudeAccuracy = (tAltitudeAccuracy > 99999) ?
					99999 + (tAltitudeAccuracy - Math.Truncate(tAltitudeAccuracy)) :
					tAltitudeAccuracy;
				tHeading = Math.Round(gpsLog.Heading ?? 0.0M, 15);
				tHeading = (tHeading > 999) ? 999 + (tHeading - Math.Truncate(tHeading)) : tHeading;
				tSpeed = Math.Round(gpsLog.Speed ?? 0.0M, 16);
				tSpeed = (tSpeed > 999) ? 999 + (tSpeed - Math.Truncate(tSpeed)) : tSpeed;

				//Package GpsLog entry
				newEntries.Add(new GpsLog {
					Latitude = tLatitude,
					Longitude = tLongitude,
					Accuracy = tAccuracy,
					Altitude = tAltitude,
					AltitudeAccuracy = tAltitudeAccuracy,
					Heading = tHeading,
					Speed = tSpeed,
					GpsTimestamp = new HelperService().UnixMillisecondsToDateTime(gpsLog.TimeStamp),
					AccId = targetAccount.AccId,

					CreatedBy = targetAccount.AccId.ToString(),
					CreatedAt = now,
					ModifiedBy = targetAccount.AccId.ToString(),
					ModifiedAt = now
				});
			}

			//Create pointsTxn to reward points for the number of logs
			now = DateTime.Now;
			var incomingPointsTxt = new AccountPointsTxn() {
				AccPointsTxnId = Guid.NewGuid(),
				AccPointsTxnAmt = newEntries.Count,

				CreatedAt = now,
				CreatedBy = "System",
				ModifiedAt = now,
				ModifiedBy = "System",

				AccId = targetAccount.AccId
			};

			//Update reward points and save pointsTxn to database
			AccountDetail targetAccDetail = await _dbContext.AccountDetails
				.Where(x => x.AccId == targetAccount.AccId)
				.FirstAsync();
			targetAccDetail.AccDetailsTotalPoints += newEntries.Count;

			//Update database
			await _dbContext.GpsLogs.AddRangeAsync(newEntries);
			await _dbContext.AccountPointsTxns.AddAsync(incomingPointsTxt);
			await _dbContext.SaveChangesAsync();

			return Ok(newEntries.Count().ToString() + " List of GPS Saved.");
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