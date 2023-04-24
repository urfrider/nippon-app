using System;
namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels
{
    public class MetadataGPSCount
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Count { get; set; }
    }

    public class MetadataPublicTransportCount
    {
        public int? PublicTransport { get; set; }
        public int? Quantity { get; set; }
    }

    public class MetadataResponsesCount
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Count { get; set; }
    }
}

