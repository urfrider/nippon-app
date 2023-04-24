namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels;

public class GpsLogInput {
	public decimal Latitude { get; set; }
	public decimal Longitude { get; set; }

	public decimal? Altitude { get; set; }

	public decimal? Accuracy { get; set; }
	public decimal? AltitudeAccuracy { get; set; }
	public decimal? Heading { get; set; }
	public decimal? Speed { get; set; }
	public long TimeStamp { get; set; }

	public DateTime? CreatedAt { get; set; }
	public string? CreatedBy { get; set; }

	public DateTime? ModifiedAt { get; set; }
	public string? ModifiedBy { get; set; }
}