using ActivitySurveyAppForSmartCityPlanning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class AccountPointsTxnController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public AccountPointsTxnController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	[HttpGet, Authorize]
	public List<AccountPointsTxn> GetAll() {
		try {
			List<AccountPointsTxn> allAccountPointsTxns = _dbContext.AccountPointsTxns.Where(x => String.Equals(x.DeletedAt, null)).ToList();

			return allAccountPointsTxns;
		}
		catch {
			throw;
		}
	}
	[HttpGet, Route("{id}"), Authorize]
	public AccountPointsTxn? GetById(string id) {
		try {
			AccountPointsTxn? selectedAccPointsTxn = _dbContext.AccountPointsTxns
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();

			return selectedAccPointsTxn;
		}
		catch {
			throw;
		}
	}

	[HttpPost, AllowAnonymous]
	public bool Create(
		[FromBody] AccountPointsTxn accPointsTxn,
		string createdBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			accPointsTxn.CreatedBy = createdBy;
			accPointsTxn.CreatedAt = DateTime.Now;
			accPointsTxn.ModifiedBy = createdBy;
			accPointsTxn.ModifiedAt = DateTime.Now;

			_dbContext.AccountPointsTxns.Add(accPointsTxn);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPut, Authorize]
	public bool Update(
		[FromBody] AccountPointsTxn accPointsTxn,
		string modifiedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			accPointsTxn.ModifiedBy = modifiedBy;
			accPointsTxn.ModifiedAt = DateTime.Now;

			_dbContext.AccountPointsTxns.Update(accPointsTxn);
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
			AccountPointsTxn? targetAccPointsTxn = _dbContext.AccountPointsTxns
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();
			if (targetAccPointsTxn == null) return false;

			targetAccPointsTxn.DeletedBy = deletedBy;
			targetAccPointsTxn.DeletedAt = DateTime.Now;
			targetAccPointsTxn.ModifiedBy = deletedBy;
			targetAccPointsTxn.ModifiedAt = DateTime.Now;

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}
	#endregion API Method(s)
}