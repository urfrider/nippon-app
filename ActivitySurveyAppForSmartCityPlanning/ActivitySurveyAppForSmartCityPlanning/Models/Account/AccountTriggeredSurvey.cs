namespace ActivitySurveyAppForSmartCityPlanning.Models;

public partial class AccountTriggeredSurvey {
	public Guid AccTriggerId { get; set; }

	//0: Available/Pending
	//1: Completed
	//2: Expired
	public int? Status { get; set; }

	//Triggered user & survey
	public Guid? SurveyId { get; set; }
	public Guid? AccId { get; set; }

	//Triggering factor details
	public Guid? TriggerId { get; set; } //SurveyTrigger.cs
	public Guid? GpsId { get; set; }

	//Triggering trip
	public string? GpsLogIds { get; set; }

	public DateTime? DeletedAt { get; set; }
	public string? DeletedBy { get; set; }
	public DateTime? CreatedAt { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime? ModifiedAt { get; set; }
	public string? ModifiedBy { get; set; }
	public DateTime? ExpireBy { get; set; }

	public virtual Account? Acc { get; set; }
	public virtual GpsLog? Gps { get; set; }
	public virtual Survey? Survey { get; set; }
	public virtual SurveyTrigger? Trigger { get; set; }
}