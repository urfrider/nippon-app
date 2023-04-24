namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels {
	public class SurveyCheckTrigger {
		// From Survey
		public Guid SurveyId { get; set; }
		public int? SurveyNoOfQns { get; set; } = 0;
		public string? SurveyTitle { get; set; }
		public bool? SurveyDisable { get; set; }
		public int? SurveyPoints { get; set; } = 0;
	}
}