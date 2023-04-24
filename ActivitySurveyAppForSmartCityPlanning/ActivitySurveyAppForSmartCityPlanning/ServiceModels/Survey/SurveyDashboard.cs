namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels.Survey;
    public class SurveyDashboard
    {
        public Guid SurveyId { get; set; }
        public int SurveyNoOfQns { get; set; }
        public string? SurveyTitle { get; set; }
        public bool SurveyDisable { get; set; }
        public int SurveyPoints { get; set; }
        public string? SurveyDesc { get; set; }
        public int NumResponses { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
    }

