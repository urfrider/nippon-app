using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class RewardController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public RewardController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region Helper Method(s)
	private string? GetAccIdFromToken(Claim[] claims) {
		try {
			string? accId = claims
				.Where(x => x.Type == ClaimTypes.NameIdentifier)
				.Select(x => x.Value)
				.SingleOrDefault();
			return accId;
		}
		catch { throw; }
	}
	#endregion Helper Method(s)

	#region API Method(s)
	[HttpGet, Route("AllRewards"), Authorize]
	public List<Reward> GetAll() {
		try {
			List<Reward> allRewards = _dbContext.Rewards.Where(x => string.Equals(x.DeletedAt, null)).ToList();

			return allRewards;
		}
		catch {
			throw;
		}
	}

	[HttpGet, Route("{rewardId}"), Authorize]
	public Reward? GetById(string rewardId) {
		try {
			Reward? selectedReward = _dbContext.Rewards
				.Where(x => string.Equals(x.RewardId.ToString(), rewardId))
				.FirstOrDefault();

			return selectedReward;
		}
		catch {
			throw;
		}
	}

	[HttpPost, Route("CreateReward"), Authorize(Roles = "DashboardUser")]
	public bool Create(
		[FromBody] Reward reward,
		string createdBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			reward.CreatedBy = createdBy;
			reward.CreatedAt = DateTime.Now;
			reward.ModifiedBy = createdBy;
			reward.ModifiedAt = DateTime.Now;

			_dbContext.Rewards.Add(reward);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPut, Route("UpdateReward"), Authorize(Roles = "DashboardUser")]
	public bool Update(
		[FromBody] Reward reward,
		string modifiedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			reward.ModifiedBy = modifiedBy;
			reward.ModifiedAt = DateTime.Now;

			_dbContext.Rewards.Update(reward);
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
			Reward? targetReward = _dbContext.Rewards
				.Where(x => string.Equals(x.RewardId.ToString(), id))
				.FirstOrDefault();
			if (targetReward == null) return false;

			targetReward.DeletedBy = deletedBy;
			targetReward.DeletedAt = DateTime.Now;
			targetReward.ModifiedBy = deletedBy;
			targetReward.ModifiedAt = DateTime.Now;

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPost, Route("ClaimReward"), Authorize(Roles = "MobileUser")]
	public IActionResult ClaimReward([FromBody] ClaimReward mobileRewardClaim) {
		try {
			#region Account ID Retrieval
			//Extract token from request
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity == null)
				return StatusCode(401, new ErrorExceptionHelper().Exception401(0));

			//Extract claims from token
			Claim[] claims = identity.Claims.ToArray();

			//Extract AccId from claims
			string accId = GetAccIdFromToken(claims) ?? "";
			if (string.IsNullOrEmpty(accId)) return StatusCode(401, new ErrorExceptionHelper().Exception401(1));
			#endregion Account ID Retrieval

			#region Target Account and Reward Retrieval
			//Getting target account to retrieve number of points
			AccountDetail? targetAccount = _dbContext.AccountDetails
				   .Where(x => string.Equals(x.AccId.ToString(), accId))
				   .FirstOrDefault();
			if (targetAccount == null) {
				return StatusCode(401, new ErrorExceptionHelper().Exception401(3));
			}

			//Getting target reward
			Reward? targetReward = _dbContext.Rewards
				   .Where(x => string.Equals(x.RewardId.ToString(), mobileRewardClaim.RewardId))
				   .FirstOrDefault();
			if (targetReward == null) {
				return StatusCode(401, new ErrorExceptionHelper().Exception401(3));
			}
			#endregion Target Account and Reward Retrieval

			#region Points and Quantity Calculation

			//Validation checks fail
			if (targetReward.RewardPoints * mobileRewardClaim.Quantity > targetAccount.AccDetailsTotalPoints ||
				targetReward.RewardQty <= 0 ||
				targetReward.RewardQty < mobileRewardClaim.Quantity) {
				return StatusCode(401, new ErrorExceptionHelper().Exception401(9));
			}
			else {
				//Updating account points
				targetAccount.AccDetailsTotalPoints -= targetReward.RewardPoints * mobileRewardClaim.Quantity;

				//Updating reward quantity
				targetReward.RewardQty -= mobileRewardClaim.Quantity;

				//Inserting details into account_pointstxn
				Guid guid = new Guid();
				DateTime now = DateTime.Now;

				AccountPointsTxn accPointsTxn = new AccountPointsTxn() {
					AccPointsTxnId = guid,
					AccId = targetAccount.AccId,
					AccPointsTxnAmt = (int)targetReward.RewardPoints * mobileRewardClaim.Quantity,
					RewardId = targetReward.RewardId,
					CreatedAt = now,
					CreatedBy = targetAccount.AccId.ToString()
				};

				_dbContext.Add(accPointsTxn);
				_dbContext.SaveChanges();

				return Ok("Reward(s) claimed.");
			}
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}

		#endregion Points and Quantity Calculation
	}
	#endregion API Method(s)
}