using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning._Scaffold;

public partial class AccountEmployment
{
    public Guid AccEmpId { get; set; }

    public string? AccEmpStatus { get; set; }

    public string? AccEmpOccupation { get; set; }

    public string? AccEmpLocation { get; set; }

    public int? AccEmpStartTime { get; set; }

    public int? AccEmpEndTime { get; set; }

    public int? AccEmpAnnualSalary { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public Guid? AccId { get; set; }

    public virtual Account? Acc { get; set; }
}
