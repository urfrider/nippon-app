namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels;

public class Profile_Post {
	public string? Username { get; set; }
	public string? Password { get; set; }
}

public class ProfileDetails_Post {
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public int? Gender { get; set; }
	public int? Age { get; set; }
	public byte[]? ProfilePicture { get; set; }
	public string? PhoneCountryCode { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Country { get; set; }
	public string? City { get; set; }
	public string? ZipCode { get; set; }
	public string? StreetAddress { get; set; }
}

public class ProfileEmployment_Post {
	public string? Status { get; set; }
	public string? Occupation { get; set; }
	public string? Location { get; set; }
	public int? StartTime { get; set; }
	public int? EndTime { get; set; }
	public int? AnnualSalary { get; set; }
}

public class ProfileExtras_Post {
	public string? DriverLicense { get; set; }
	public int? MobilityImpaired { get; set; }
	public string? HouseholdPosition { get; set; }
	public int? NoOfVehicles { get; set; }
}