using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning._Scaffold;

public partial class AccountTriggeredSurvey
{
    public Guid AccTriggerId { get; set; }

    public string? GpsLogIds { get; set; }

    public int? Status { get; set; }

    public Guid? SurveyId { get; set; }

    public Guid? AccId { get; set; }

    public Guid? TriggerId { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ExpireBy { get; set; }

    public Guid? GpsId { get; set; }

    public virtual Account? Acc { get; set; }

    public virtual GpsLog? Gps { get; set; }

    public virtual Survey? Survey { get; set; }

    public virtual SurveyTrigger? Trigger { get; set; }
}
