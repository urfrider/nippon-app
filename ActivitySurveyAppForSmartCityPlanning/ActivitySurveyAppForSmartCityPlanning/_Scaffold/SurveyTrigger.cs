using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning._Scaffold;

public partial class SurveyTrigger
{
    public Guid TriggerId { get; set; }

    public string? LatLong { get; set; }

    public int? TriggerRadius { get; set; }

    public int? TriggerCooldown { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public Guid? SurveyId { get; set; }

    public virtual ICollection<AccountTriggeredSurvey> AccountTriggeredSurveys { get; } = new List<AccountTriggeredSurvey>();

    public virtual Survey? Survey { get; set; }
}
