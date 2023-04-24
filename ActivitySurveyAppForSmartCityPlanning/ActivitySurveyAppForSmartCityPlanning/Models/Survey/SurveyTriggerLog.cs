using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning.Models;

public partial class SurveyTriggerLog
{
    public Guid SurveyTriggerLogId { get; set; }

    public DateTime? LogDateTime { get; set; }
}
