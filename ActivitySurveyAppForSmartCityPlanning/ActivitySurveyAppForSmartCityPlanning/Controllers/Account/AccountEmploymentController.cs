using ActivitySurveyAppForSmartCityPlanning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class AccountEmploymentController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public AccountEmploymentController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	[HttpGet, Authorize]
	public List<AccountEmployment> GetAll() {
		try {
			List<AccountEmployment> allAccEmployment = _dbContext.AccountEmployments.Where(x => String.Equals(x.DeletedAt, null)).ToList();

			return allAccEmployment;
		}
		catch {
			throw;
		}
	}
	[HttpGet, Route("{id}"), Authorize]
	public AccountEmployment? GetById(string id) {
		try {
			AccountEmployment? selectedAccEmp = _dbContext.AccountEmployments
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();

			return selectedAccEmp;
		}
		catch {
			throw;
		}
	}

	[HttpPost, AllowAnonymous]
	public bool Create(
		[FromBody] AccountEmployment accEmployment,
		string createdBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			accEmployment.CreatedBy = createdBy;
			accEmployment.CreatedAt = DateTime.Now;
			accEmployment.ModifiedBy = createdBy;
			accEmployment.ModifiedAt = DateTime.Now;

			_dbContext.AccountEmployments.Add(accEmployment);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPut, Authorize]
	public bool Update(
		[FromBody] AccountEmployment accEmployment,
		string modifiedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			accEmployment.ModifiedBy = modifiedBy;
			accEmployment.ModifiedAt = DateTime.Now;

			_dbContext.AccountEmployments.Update(accEmployment);
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
			AccountEmployment? targetAccEmployment = _dbContext.AccountEmployments
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();
			if (targetAccEmployment == null) return false;

			targetAccEmployment.DeletedBy = deletedBy;
			targetAccEmployment.DeletedAt = DateTime.Now;
			targetAccEmployment.ModifiedBy = deletedBy;
			targetAccEmployment.ModifiedAt = DateTime.Now;

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}
	#endregion API Method(s)
}