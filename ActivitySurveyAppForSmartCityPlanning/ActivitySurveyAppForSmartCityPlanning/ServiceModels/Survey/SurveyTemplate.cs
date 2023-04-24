namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels;

public class SurveyTemplate {
	public string? Id { get; set; }

	public string? Title { get; set; }
	public string? Description { get; set; }
	public int Points { get; set; } = 0;

	public string? LatLong { get; set; }

	public string? Latitude { get; set; }
	public string? Longitude { get; set; }

	public int? Radius { get; set; }
	public int? Cooldown { get; set; }
	public int? Expiry { get; set; }

	public int IsTravelling { get; set; } = 0;
	public int IsTravellingOnPublicTransport { get; set; } = 0;
	public decimal[][]? TripLatLongs { get; set; }
	public decimal[]? TriggeredLatLong { get; set; }
	public int TriggeredPos { get; set; } = 0;

	public SurveyQuestionTemplate[]? Questions { get; set; }
}

public class SurveyQuestionTemplate {
	public string? QuestionId { get; set; }
	public string? Question { get; set; }
	public int Type { get; set; } = 0;
	public string[]? Options { get; set; }
}