namespace ActivitySurveyAppForSmartCityPlanning.Models;

public partial class ResponseQuestion {
	public Guid ResQnsId { get; set; }
	public int ResQnsType { get; set; }
	public string? ResQnsString { get; set; }
	public int? ResQnsInt { get; set; }
	public decimal? ResQnsDecimal { get; set; }

	public DateTime? DeletedAt { get; set; }
	public string? DeletedBy { get; set; }

	public DateTime? CreatedAt { get; set; }
	public string? CreatedBy { get; set; }

	public DateTime? ModifiedAt { get; set; }
	public string? ModifiedBy { get; set; }

	public Guid? ResId { get; set; }
	public Guid? QnsId { get; set; }

	public virtual SurveyQuestion? Qns { get; set; }
	public virtual Response? Res { get; set; }
}