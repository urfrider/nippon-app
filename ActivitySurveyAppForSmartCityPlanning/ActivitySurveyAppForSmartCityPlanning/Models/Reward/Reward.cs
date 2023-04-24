namespace ActivitySurveyAppForSmartCityPlanning.Models;

public partial class Reward {
	public Guid RewardId { get; set; }

	public string? RewardName { get; set; }
	public string? RewardDesc { get; set; }
	public int? RewardQty { get; set; }
	public byte[]? RewardImg { get; set; }
	public int? RewardPoints { get; set; }

	public DateTime? DeletedAt { get; set; }
	public string? DeletedBy { get; set; }
	public DateTime? CreatedAt { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime? ModifiedAt { get; set; }
	public string? ModifiedBy { get; set; }

	public virtual ICollection<AccountPointsTxn> AccountPointsTxns { get; } = new List<AccountPointsTxn>();
}