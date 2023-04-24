using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers.Points {
	[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
	public class PointsManagementController : Controller {
		private TravelRewardsContext _dbContext;

		#region Class Constructor(s)
		public PointsManagementController(
			TravelRewardsContext dbContextInjection) {
			_dbContext = dbContextInjection;
		}
		#endregion Class Constructor(s)

		#region API Method(s)
		// Admin triggered to recalculate everyone
		[HttpPost, Route("CalculateAll"), Authorize(Roles = "Admin")]
		public async Task<IActionResult> CalculateAllPoints() {
			try {
				Dictionary<Guid?, int> PointsDict = new Dictionary<Guid?, int>();
				List<AccountPointsTxn> allPoints = _dbContext.AccountPointsTxns.ToList();
				List<AccountDetail> accountDetails = _dbContext.AccountDetails.ToList();

				// Consolidate
				foreach (AccountPointsTxn points in allPoints) {
					if (!PointsDict.ContainsKey(points.AccId))
						PointsDict.Add(points.AccId, points.AccPointsTxnAmt);
					else {
						//int currentPoints = PointsDict[points.AccId];
						//int newPoints = points.AccPointsTxnAmt + currentPoints;
						//PointsDict[points.AccId] = newPoints;

						PointsDict[points.AccId] += points.AccPointsTxnAmt;
					}
				}

				for (int i = 0; i < accountDetails.Count; i++) {
					string? accountId = accountDetails[i].AccId.ToString();
					accountDetails[i].AccDetailsTotalPoints = PointsDict[accountDetails[i].AccId];

					AccountDetail? targetAccountDetail = await _dbContext.AccountDetails
						.Where(x => string.Equals(x.AccId.ToString(), accountId)).FirstOrDefaultAsync();
				}

				_dbContext.SaveChanges();

				return Ok("Successfully Calculated All.");
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
}