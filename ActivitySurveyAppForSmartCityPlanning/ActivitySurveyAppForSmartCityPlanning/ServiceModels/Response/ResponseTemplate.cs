namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels;

public class ResponseTemplate {
	public string SurveyId { get; set; }
	public int isTravellingOnPublicTransport { get; set; } = 0;
	public ResponseQuestionTemplate[]? ResQns { get; set; }
}

public class ResponseQuestionTemplate {
	public string SurveyQnsId { get; set; }
	public int SurveyQnsType { get; set; }
	public string SurveyQnsRes { get; set; }
}