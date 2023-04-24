namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels;

public class GPSDataFilter {
	public int StartDate_Day { get; set; } = DateTime.Now.Day;
	public int StartDate_Month { get; set; } = DateTime.Now.Month;
	public int StartDate_Year { get; set; } = DateTime.Now.Year;

	public int EndDate_Day { get; set; } = DateTime.Now.Day;
	public int EndDate_Month { get; set; } = DateTime.Now.Month;
	public int EndDate_Year { get; set; } = DateTime.Now.Year;

	public int? StartTime_Hours { get; set; }
	public int? StartTime_Minutes { get; set; } = 0;

	public int? EndTime_Hours { get; set; }
	public int? EndTime_Minutes { get; set; } = 0;
}