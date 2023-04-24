namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels;

public class RegisterAccount {
	public string? Username { get; set; }
	public string? Password { get; set; }
	public int Role { get; set; }
	public string? CreatedBy { get; set; }

	//Account Details
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public int Gender { get; set; }
	public int? Age { get; set; }
	public string? PhoneCountryCode { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Country { get; set; }
	public string? City { get; set; }
	public string? ZipCode { get; set; }
	public string? StreetAddress { get; set; }

	//Employment Details
	public string? EmploymentStatus { get; set; }
	public string? Occupation { get; set; }
	public string? EmploymentLocation { get; set; }
	public int? EmploymentStartTime { get; set; }
	public int? EmploymentEndTime { get; set; }
	public int? EmploymentAnnualSalary { get; set; }

	//Account Extra Details
	public string? DriverLicense { get; set; }
	public int MobilityImpaired { get; set; }
	public string? HouseholdPosition { get; set; }
	public int? NoOfVehicles { get; set; }
}