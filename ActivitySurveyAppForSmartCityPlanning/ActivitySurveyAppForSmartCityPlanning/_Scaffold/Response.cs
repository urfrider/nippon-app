using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning._Scaffold;

public partial class Response
{
    public Guid ResId { get; set; }

    public int ResNoOfQns { get; set; }

    public bool ResponseDisable { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public Guid? SurveyId { get; set; }

    public Guid? AccId { get; set; }

    public int? ByPublicTransport { get; set; }

    public virtual Account? Acc { get; set; }

    public virtual ICollection<ResponseQuestion> ResponseQuestions { get; } = new List<ResponseQuestion>();

    public virtual Survey? Survey { get; set; }
}
