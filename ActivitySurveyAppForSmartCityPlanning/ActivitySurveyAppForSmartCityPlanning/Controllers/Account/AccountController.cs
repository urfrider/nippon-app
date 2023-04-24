using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels.Profile;
using ActivitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class AccountController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public AccountController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	[HttpGet, Authorize]
	public List<Account> GetAll() {
		try {
			List<Account> allAccounts = _dbContext.Accounts.Where(x => String.Equals(x.AccDisable, false)).ToList();

			return allAccounts;
		}
		catch {
			throw;
		}
	}
	[HttpGet, Route("{id}"), Authorize]
	public Account? GetById(string id) {
		try {
			Account? selectedAccount = _dbContext.Accounts
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();

			return selectedAccount;
		}
		catch {
			throw;
		}
	}

	[HttpPost, AllowAnonymous]
	public bool Create(
		[FromBody] Account account,
		string createdBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			account.CreatedBy = createdBy;
			account.CreatedAt = DateTime.Now;
			account.ModifiedBy = createdBy;
			account.ModifiedAt = DateTime.Now;

			_dbContext.Accounts.Add(account);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPut, Authorize]
	public bool Update(
		[FromBody] Account account,
		string modifiedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			account.ModifiedBy = modifiedBy;
			account.ModifiedAt = DateTime.Now;

			_dbContext.Accounts.Update(account);
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
			Account? targetAccount = _dbContext.Accounts
				.Where(x => string.Equals(x.AccId.ToString(), id))
				.FirstOrDefault();
			if (targetAccount == null) return false;

			targetAccount.AccDisable = true;
			targetAccount.DeletedBy = deletedBy;
			targetAccount.DeletedAt = DateTime.Now;
			targetAccount.ModifiedBy = deletedBy;
			targetAccount.ModifiedAt = DateTime.Now;

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpGet, Route("GetAllMobileUsers"), Authorize(Roles = "DashboardUser")]
	public async Task<IActionResult> GetAllMobileUsers() {
		try {
			List<MobileUser> mobileUsers = await (
				from x in _dbContext.Accounts
				join y in _dbContext.AccountDetails
				on x.AccId equals y.AccId
				where (x.AccRole == 0 && String.Equals(x.AccDisable, false))
				select new MobileUser {
					AccId = x.AccId,
					AccUsername = x.AccUsername,
					AccDetailsTotalPoints = y.AccDetailsTotalPoints
				}).ToListAsync();

			return Ok(mobileUsers);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception401(0) + e.Message.ToString());
		}
	}

	[HttpGet, Route("GetAllDashboardUsers"), Authorize(Roles = "DashboardUser")]
	public async Task<IActionResult> GetAllDashboardUsers() {
		try {
			List<Account> allAccounts = await _dbContext.Accounts
				.Where(x => x.AccRole == 1 && String.Equals(x.AccDisable, false))
				.ToListAsync();

			return Ok(allAccounts);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception401(0) + e.Message.ToString());
		}
	}
	[HttpGet, Route("GetAllAdmins"), Authorize(Roles = "DashboardUser")]
	public async Task<IActionResult> GetAllAdmins() {
		try {
			List<Account> allAccounts = await _dbContext.Accounts
				.Where(x => x.AccRole == 2)
				.ToListAsync();

			return Ok(allAccounts);
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception401(0) + e.Message.ToString());
		}
	}
	#endregion API Method(s)
}