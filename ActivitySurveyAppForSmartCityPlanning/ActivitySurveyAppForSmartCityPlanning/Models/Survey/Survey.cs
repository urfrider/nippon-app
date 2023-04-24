namespace ActivitySurveyAppForSmartCityPlanning.Models;

public partial class Survey {
	public Guid SurveyId { get; set; }

	public int SurveyNoOfQns { get; set; }
	public string SurveyTitle { get; set; } = null!;

	public bool SurveyDisable { get; set; }
	public int SurveyPoints { get; set; }
	public string SurveyDesc { get; set; } = null!;

	public DateTime? DeletedAt { get; set; }
	public string? DeletedBy { get; set; }

	public DateTime? CreatedAt { get; set; }
	public string? CreatedBy { get; set; }

	public DateTime? ModifiedAt { get; set; }
	public string? ModifiedBy { get; set; }

	public virtual ICollection<AccountTriggeredSurvey> AccountTriggeredSurveys { get; } = new List<AccountTriggeredSurvey>();
	public virtual ICollection<Response> Responses { get; } = new List<Response>();
	public virtual ICollection<SurveyQuestion> SurveyQuestions { get; } = new List<SurveyQuestion>();
	public virtual ICollection<SurveyTrigger> SurveyTriggers { get; } = new List<SurveyTrigger>();
}