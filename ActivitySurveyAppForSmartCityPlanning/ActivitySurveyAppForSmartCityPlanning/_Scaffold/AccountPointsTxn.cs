using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning._Scaffold;

public partial class AccountPointsTxn
{
    public Guid AccPointsTxnId { get; set; }

    public int AccPointsTxnAmt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public Guid? AccId { get; set; }

    public Guid? RewardId { get; set; }

    public virtual Account? Acc { get; set; }

    public virtual Reward? Reward { get; set; }
}
