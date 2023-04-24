namespace ActivitySurveyAppForSmartCityPlanning;

public class AppSettings {
	public JwtConfig? JwtConfig { get; set; }
	public DBConfig? DBConfig { get; set; }
	public AccountConfig? AccountConfig { get; set; }
}

public class JwtConfig {
	public string? JwtSymmetricKey { get; set; }
	public string? JwtIssuer { get; set; }
	public string? JwtAudience { get; set; }
	public int? JwtExpiryFromNow { get; set; }
	public string? JwtTokenKey { get; set; }
}

public class DBConfig {
	public string? DBEndpoint { get; set; }
	public string? DBDatabase { get; set; }
	public string? DBUsername { get; set; }
	public string? DBPassword { get; set; }
}

public class AccountConfig {
	public string? AccountPasswordSalt { get; set; }
}