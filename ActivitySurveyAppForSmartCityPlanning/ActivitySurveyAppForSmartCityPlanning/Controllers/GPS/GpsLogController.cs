using ActivitySurveyAppForSmartCityPlanning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers.GPS;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class GpsLogController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public GpsLogController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	[HttpGet, Authorize(Roles = "DashboardUser")]
	public List<GpsLog> GetAll() {
		try {
			List<GpsLog> allGpsLogs = _dbContext.GpsLogs.ToList();

			return allGpsLogs;
		}
		catch {
			throw;
		}
	}
	[HttpGet, Route("{id}"), Authorize(Roles = "DashboardUser")]
	public GpsLog? GetById(string id) {
		try {
			GpsLog? selectedLog = _dbContext.GpsLogs
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();

			return selectedLog;
		}
		catch {
			throw;
		}
	}

	[HttpPost, Authorize]
	public bool Create(
		[FromBody] GpsLog gpsLog,
		string createdBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			gpsLog.CreatedBy = createdBy;
			gpsLog.CreatedAt = DateTime.Now;
			gpsLog.ModifiedBy = createdBy;
			gpsLog.ModifiedAt = DateTime.Now;

			_dbContext.GpsLogs.Add(gpsLog);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPut, Authorize(Roles = "DashboardUser")]
	public bool Update(
		[FromBody] GpsLog gpsLog,
		string modifiedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			gpsLog.ModifiedBy = modifiedBy;
			gpsLog.ModifiedAt = DateTime.Now;

			_dbContext.GpsLogs.Update(gpsLog);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpDelete, Route("{gpsId}"), Authorize(Roles = "Admin")]
	public bool Delete(
		string gpsId,
		string deletedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			GpsLog? targetLog = _dbContext.GpsLogs
				.Where(x => string.Equals(x.GpsId.ToString(), gpsId))
				.FirstOrDefault();
			if (targetLog == null) return false;

			targetLog.DeletedBy = deletedBy;
			targetLog.DeletedAt = DateTime.Now;
			targetLog.ModifiedBy = deletedBy;
			targetLog.ModifiedAt = DateTime.Now;

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}
	#endregion API Method(s)
}