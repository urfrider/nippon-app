using ActivitySurveyAppForSmartCityPlanning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class AccountExtraController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public AccountExtraController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	[HttpGet, Authorize]
	public List<AccountExtra> GetAll() {
		try {
			List<AccountExtra> allAccExtras = _dbContext.AccountExtras.Where(x => String.Equals(x.DeletedAt, null)).ToList();

			return allAccExtras;
		}
		catch {
			throw;
		}
	}
	[HttpGet, Route("{id}"), Authorize]
	public AccountExtra? GetById(string id) {
		try {
			AccountExtra? selectedAccExtra = _dbContext.AccountExtras
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();

			return selectedAccExtra;
		}
		catch {
			throw;
		}
	}

	[HttpPost, AllowAnonymous]
	public bool Create([
		FromBody] AccountExtra accExtra,
		string createdBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			accExtra.CreatedBy = createdBy;
			accExtra.CreatedAt = DateTime.Now;
			accExtra.ModifiedBy = createdBy;
			accExtra.ModifiedAt = DateTime.Now;

			_dbContext.AccountExtras.Add(accExtra);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPut, Authorize]
	public bool Update(
		[FromBody] AccountExtra accExtra,
		string modifiedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			accExtra.ModifiedBy = modifiedBy;
			accExtra.ModifiedAt = DateTime.Now;

			_dbContext.AccountExtras.Update(accExtra);
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
			AccountExtra? targetAccExtra = _dbContext.AccountExtras
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();
			if (targetAccExtra == null) return false;

			targetAccExtra.DeletedBy = deletedBy;
			targetAccExtra.DeletedAt = DateTime.Now;
			targetAccExtra.ModifiedBy = deletedBy;
			targetAccExtra.ModifiedAt = DateTime.Now;

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}
	#endregion API Method(s)
}