using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers.HeatMap;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class HeatMapController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public HeatMapController(
		IOptions<AppSettings> appSettingsInjection,
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	[HttpPost, Authorize(Roles = "DashboardUser")]
	public async Task<IActionResult> RetrieveAllUserGPSData([FromBody] GPSDataFilter inputFilter) {
		try {
			#region GPS Logs Filtering
			//Construct DateOnly
			DateOnly inputStartDate = new DateOnly(
					inputFilter.StartDate_Year,
					inputFilter.StartDate_Month,
					inputFilter.StartDate_Day
			);
			DateOnly inputEndDate = new DateOnly(
				inputFilter.EndDate_Year,
				inputFilter.EndDate_Month,
				inputFilter.EndDate_Day
			);

			//Acquire GPS logs filtered by date
			List<GpsLog> targetLogs = await _dbContext.GpsLogs.ToListAsync();

			targetLogs = targetLogs
				.Where(x => inputStartDate.CompareTo(DateOnly.FromDateTime(x.GpsTimestamp.Date)) <= 0)
				.Where(y => inputEndDate.CompareTo(DateOnly.FromDateTime(y.GpsTimestamp.Date)) >= 0)
				.ToList();

			//Acquire GPS logs filtered by time
			if ((inputFilter.StartTime_Hours != null) && (inputFilter.EndTime_Hours != null)) {
				//Construct TimeOnly
				TimeOnly inputStartTime = new TimeOnly(
					inputFilter.StartTime_Hours ?? 0,
					inputFilter.StartTime_Minutes ?? 0);
				TimeOnly inputEndTime = new TimeOnly(
					inputFilter.EndTime_Hours ?? 23,
					inputFilter.EndTime_Minutes ?? 59);

				targetLogs = targetLogs
					.Where(x => inputStartTime >= (TimeOnly.FromDateTime(x.GpsTimestamp)))
					.Where(y => inputEndTime <= (TimeOnly.FromDateTime(y.GpsTimestamp)))
					.ToList();
			}
			#endregion GPS Logs Filtering

			#region DBH Calculation
			//DBH value calculation, for increasing heatmap intensity if same coordinates appears
			List<decimal[]> distinctCoords = new List<decimal[]>();
			List<int> distinctCoordsDBH = new List<int>();
			int position = -1;
			foreach (GpsLog targetLog in targetLogs) {
				decimal[] currentCoords = new decimal[] {
					Math.Round(targetLog.Longitude, 5),
					Math.Round(targetLog.Latitude, 5)
				};

				//Check if currentCoords is distinct
				position = distinctCoords
					.FindIndex(x => x.SequenceEqual(currentCoords));

				//currentCoords is distinct
				if (position < 0) {
					distinctCoords.Add(currentCoords);
					distinctCoordsDBH.Add(30);
				}
				//currentCoords is not distinct
				else distinctCoordsDBH[position] += (distinctCoordsDBH[position] >= 64) ? 0 : 1;
			}
			#endregion DBH Calculation

			#region Transform Records to Service Model
			//Initialize feature collection
			FeatureCollection returnData = new FeatureCollection();
			List<Feature> returnDataFeatures = new List<Feature>();

			//Map coordinates to service model
			decimal[][] distinctCoordsArr = distinctCoords.ToArray();
			int[] distinctCoordsDBHArr = distinctCoordsDBH.ToArray();
			for (int i = 0; i < distinctCoordsDBHArr.Length; i++) {
				returnDataFeatures.Add(
					new Feature(
						new FeatureProperties(distinctCoordsDBHArr[i]),
						new FeatureGeometry(
							distinctCoordsArr[i][0],
							distinctCoordsArr[i][1]
						)
					)
				);
			}
			returnData.features = returnDataFeatures.ToArray();
			#endregion Transform Records to Service Model

			return Ok(returnData);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}
}