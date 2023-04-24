namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels;

public class Profile_Get
{
    public string? Username { get; set; }
    public int Role { get; set; } = 0;

    public string? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}

public class ProfileDetails_Get
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Gender { get; set; } = 0;
    public int? Age { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public int TotalPoints { get; set; } = 0;
    public string? PhoneCountryCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? StreetAddress { get; set; }

    public string? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}

public class ProfileEmployment_Get
{
    public string? Status { get; set; }
    public string? Occupation { get; set; }
    public string? Location { get; set; }
    public int? StartTime { get; set; }
    public int? EndTime { get; set; }
    public int? AnnualSalary { get; set; }

    public string? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}

public class ProfileExtras_Get
{
    public string? DriverLicense { get; set; }
    public int MobilityImpaired { get; set; }
    public string? HouseholdPosition { get; set; }
    public int? NoOfVehicles { get; set; }

    public string? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}

public class ProfileAll
{
    public Guid accId { get; set; }
    public string? Username { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Gender { get; set; } = 0;
    public int? Age { get; set; }

    public int? TotalPoints { get; set; } = 0;
    public string? PhoneCountryCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? StreetAddress { get; set; }

    public string? Status { get; set; }
    public string? Occupation { get; set; }
    public string? EmploymentLocation { get; set; }
    public int? StartTime { get; set; }
    public int? EndTime { get; set; }
    public int? AnnualSalary { get; set; }

    public string? DriverLicense { get; set; }
    public bool? MobilityImpaired { get; set; }
    public string? HouseholdPosition { get; set; }
    public int? NoOfVehicles { get; set; }

}