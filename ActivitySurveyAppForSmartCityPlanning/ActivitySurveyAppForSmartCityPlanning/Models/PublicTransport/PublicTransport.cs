using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning.Models;

public partial class PublicTransport
{
    public int TransportId { get; set; }

    public string? TransportName { get; set; }

    public decimal? TransportLongitude { get; set; }

    public decimal? TransportLatitude { get; set; }
}
