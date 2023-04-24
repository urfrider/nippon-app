namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels.Response
{
    //survey title
    //question in each survey, responses for the question
    public class ResponsesDisplay
    {
        public Guid SurveyId { get; set; }
        public string SurveyTitle { get; set; } = null!;

        public Guid QnsId { get; set; }
        public string Qns { get; set; } = null!;
        public int QnsType { get; set; }
        public string? QnsOption01 { get; set; }
        public string? QnsOption02 { get; set; }
        public string? QnsOption03 { get; set; }
        public string? QnsOption04 { get; set; }
        public string? QnsOption05 { get; set; }
        public string? QnsOption06 { get; set; }
        public string? QnsOption07 { get; set; }
        public string? QnsOption08 { get; set; }

        public Guid ResQnsId { get; set; }
        public int ResQnsType { get; set; }
        public string? ResQnsString { get; set; }
        public int? ResQnsInt { get; set; }
        public decimal? ResQnsDecimal { get; set; }

    }
}
