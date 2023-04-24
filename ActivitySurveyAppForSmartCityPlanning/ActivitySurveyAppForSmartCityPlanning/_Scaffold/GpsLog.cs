using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning._Scaffold;

public partial class GpsLog
{
    public Guid GpsId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public decimal? Accuracy { get; set; }

    public decimal Altitude { get; set; }

    public decimal? AltitudeAccuracy { get; set; }

    public decimal? Heading { get; set; }

    public decimal? Speed { get; set; }

    public DateTime GpsTimestamp { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public Guid? AccId { get; set; }

    public virtual Account? Acc { get; set; }

    public virtual ICollection<AccountTriggeredSurvey> AccountTriggeredSurveys { get; } = new List<AccountTriggeredSurvey>();
}
