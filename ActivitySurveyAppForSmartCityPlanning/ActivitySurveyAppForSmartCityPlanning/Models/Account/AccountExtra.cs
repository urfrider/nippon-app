namespace ActivitySurveyAppForSmartCityPlanning.Models;

public partial class AccountExtra {
	public Guid AccExtraId { get; set; }

	public string? AccExtraDriverLicense { get; set; }
	public bool AccExtraMobilityImpaired { get; set; }
	public string? AccExtraHouseholdPosition { get; set; }
	public int? AccExtraNumberOfVehicles { get; set; }

	public DateTime? DeletedAt { get; set; }
	public string? DeletedBy { get; set; }
	public DateTime? CreatedAt { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime? ModifiedAt { get; set; }
	public string? ModifiedBy { get; set; }

	public Guid? AccId { get; set; }

	public virtual Account? Acc { get; set; }
}