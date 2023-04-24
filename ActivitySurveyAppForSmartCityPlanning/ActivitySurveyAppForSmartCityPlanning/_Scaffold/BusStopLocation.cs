using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning._Scaffold;

public partial class BusStopLocation
{
    public int BusId { get; set; }

    public string? BusName { get; set; }

    public decimal? BusLongitude { get; set; }

    public decimal? BusLatitude { get; set; }
}
