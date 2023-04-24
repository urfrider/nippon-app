using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning._Scaffold;

public partial class Account
{
    public Guid AccId { get; set; }

    public string AccUsername { get; set; } = null!;

    public string AccPassword { get; set; } = null!;

    public bool AccDisable { get; set; }

    public string? AccCurrentSession { get; set; }

    public int AccRole { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<AccountDetail> AccountDetails { get; } = new List<AccountDetail>();

    public virtual ICollection<AccountEmployment> AccountEmployments { get; } = new List<AccountEmployment>();

    public virtual ICollection<AccountExtra> AccountExtras { get; } = new List<AccountExtra>();

    public virtual ICollection<AccountPointsTxn> AccountPointsTxns { get; } = new List<AccountPointsTxn>();

    public virtual ICollection<AccountTriggeredSurvey> AccountTriggeredSurveys { get; } = new List<AccountTriggeredSurvey>();

    public virtual ICollection<GpsLog> GpsLogs { get; } = new List<GpsLog>();

    public virtual ICollection<Response> Responses { get; } = new List<Response>();
}
