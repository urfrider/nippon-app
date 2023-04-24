using System;
using System.Collections.Generic;

namespace ActivitySurveyAppForSmartCityPlanning._Scaffold;

public partial class AccountDetail
{
    public Guid AccDetailsId { get; set; }

    public string? AccDetailsFirstName { get; set; }

    public string? AccDetailsLastName { get; set; }

    public int? AccDetailsGender { get; set; }

    public byte[]? AccDetailsProfilePicture { get; set; }

    public int? AccDetailsTotalPoints { get; set; }

    public string? AccDetailsPhoneCountryCode { get; set; }

    public string? AccDetailsPhoneNumber { get; set; }

    public string? AccDetailsAddressCountry { get; set; }

    public string? AccDetailsAddressCity { get; set; }

    public string? AccDetailsAddressZipCode { get; set; }

    public string? AccDetailsAddressStreet { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public Guid? AccId { get; set; }

    public int? AccDetailsAge { get; set; }

    public virtual Account? Acc { get; set; }
}
