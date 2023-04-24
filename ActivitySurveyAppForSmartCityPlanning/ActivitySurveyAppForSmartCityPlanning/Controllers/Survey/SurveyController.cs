using ActivitySurveyAppForSmartCityPlanning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels.Survey;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class SurveyController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public SurveyController(
		TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
    #endregion Class Constructor(s)

    #region API Method(s)
    [HttpGet, Authorize]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var surveys = await _dbContext.Surveys
                .Include(s => s.Responses)
                .Where(x => String.Equals(x.DeletedAt, null))
                .Select(s => new SurveyDashboard
                {
                    SurveyId = s.SurveyId,
                    SurveyNoOfQns = s.SurveyNoOfQns,
                    SurveyTitle = s.SurveyTitle,
                    SurveyDisable = s.SurveyDisable,
                    SurveyPoints = s.SurveyPoints,
                    SurveyDesc = s.SurveyDesc,
                    NumResponses = s.Responses.Count,
					DeletedAt = s.DeletedAt,
					DeletedBy = s.DeletedBy,
					CreatedAt = s.CreatedAt,
					CreatedBy = s.CreatedBy,
					ModifiedAt = s.ModifiedAt,
					ModifiedBy = s.ModifiedBy
                })
                .ToListAsync();

            return Ok(surveys);
        }
        catch
        {
            throw;
        }
    }

    [HttpGet, Route("{surveyId}"), Authorize]
	public Survey? GetById(string surveyId) {
		try {
			Survey? selectedSurvey = _dbContext.Surveys
				.Where(x => string.Equals(x.SurveyId.ToString(), surveyId))
				.FirstOrDefault();

			return selectedSurvey;
		}
		catch {
			throw;
		}
	}

	[HttpPost, Authorize]
	public bool Create(
		[FromBody] Survey survey,
		string createdBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			survey.CreatedBy = createdBy;
			survey.CreatedAt = DateTime.Now;
			survey.ModifiedBy = createdBy;
			survey.ModifiedAt = DateTime.Now;

			_dbContext.Surveys.Add(survey);
			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}

	[HttpPut, Authorize]
	public bool Update(
		[FromBody] Survey survey,
		string modifiedBy = "[DIRECT_API]",
		bool saveChanges = true) {
		try {
			survey.ModifiedBy = modifiedBy;
			survey.ModifiedAt = DateTime.Now;

			_dbContext.Surveys.Update(survey);
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
			Survey? targetSurvey = _dbContext.Surveys
				.Where(x => string.Equals(x.SurveyId.ToString(), id))
				.FirstOrDefault();
			if (targetSurvey == null) return false;

			targetSurvey.DeletedBy = deletedBy;
			targetSurvey.DeletedAt = DateTime.Now;
			targetSurvey.ModifiedBy = deletedBy;
			targetSurvey.ModifiedAt = DateTime.Now;

			if (saveChanges) _dbContext.SaveChanges();

			return true;
		}
		catch {
			throw;
		}
	}
	#endregion API Method(s)
}