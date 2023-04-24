using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning._Scaffold;

public partial class SurveyQuestion
{
    public Guid QnsId { get; set; }

    public string Qns { get; set; } = null!;

    public int QnsType { get; set; }

    public int QnsOrder { get; set; }

    public string? QnsOption01 { get; set; }

    public string? QnsOption02 { get; set; }

    public string? QnsOption03 { get; set; }

    public string? QnsOption04 { get; set; }

    public string? QnsOption05 { get; set; }

    public string? QnsOption06 { get; set; }

    public string? QnsOption07 { get; set; }

    public string? QnsOption08 { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public Guid? SurveyId { get; set; }

    public virtual ICollection<ResponseQuestion> ResponseQuestions { get; } = new List<ResponseQuestion>();

    public virtual Survey? Survey { get; set; }
}
