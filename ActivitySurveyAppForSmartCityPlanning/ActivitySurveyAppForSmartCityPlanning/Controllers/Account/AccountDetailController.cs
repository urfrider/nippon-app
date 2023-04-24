using ActivitySurveyAppForSmartCityPlanning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class AccountDetailController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public AccountDetailController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	[HttpGet, Authorize]
	public List<AccountDetail> GetAll() {
		try {
			List<AccountDetail> allAccDetails = _dbContext.AccountDetails.ToList();

			return allAccDetails;
		}
		catch {
			throw;
		}
	}
	[HttpGet, Route("{id}"), Authorize]
	public AccountDetail? GetById(string id) {
		try {
			AccountDetail? selectedAccDetail = _dbContext.AccountDetails
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();

			return selectedAccDetail;
		}
		catch {
			throw;
		}
	}

	[HttpPost, AllowAnonymous]
	public bool Create(
		[FromBody] AccountDetail accDetail,
		string createdBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			accDetail.CreatedBy = createdBy;
			accDetail.CreatedAt = DateTime.Now;
			accDetail.ModifiedBy = createdBy;
			accDetail.ModifiedAt = DateTime.Now;

			_dbContext.AccountDetails.Add(accDetail);

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPut, Authorize]
	public bool Update(
		[FromBody] AccountDetail accDetail,
		string modifiedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			accDetail.ModifiedBy = modifiedBy;
			accDetail.ModifiedAt = DateTime.Now;
			_dbContext.AccountDetails.Update(accDetail);

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
			AccountDetail? targetAccDetail = _dbContext.AccountDetails
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();
			if (targetAccDetail == null) return false;

			targetAccDetail.DeletedBy = deletedBy;
			targetAccDetail.DeletedAt = DateTime.Now;
			targetAccDetail.ModifiedBy = deletedBy;
			targetAccDetail.ModifiedAt = DateTime.Now;
			_dbContext.AccountDetails.Update(targetAccDetail);

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}
	#endregion API Method(s)
}