namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels.Metadata;

public class MetadataQnsResData {
	public string Qns { get; set; }
	public int QnsType { get; set; }
	public int TotalRes { get; set; }

	public string[] labels { get; set; }
	public int[] data { get; set; }

	public string[] opendEndedData { get; set; }
}