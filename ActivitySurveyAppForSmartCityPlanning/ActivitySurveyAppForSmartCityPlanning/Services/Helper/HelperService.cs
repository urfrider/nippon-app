namespace ActivitySurveyAppForSmartCityPlanning.Services;

public class HelperService {
	//Retrieves unix timestamp and convert it to Singapore local DateTime.
	public DateTime UnixSecondsToDateTime(long timestamp) {
		return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.AddHours(8);
	}
	public DateTime UnixMillisecondsToDateTime(long timestamp) {
		return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime.AddHours(8);
	}

	//Calculate distance in kilometers between 2 coordinates
	public double CalculateDistBtwCoords(double lat01, double long01, double lat02, double long02) {
		double rlat1 = Math.PI * lat01 / 180;
		double rlat2 = Math.PI * lat02 / 180;
		double theta = long01 - long02;
		double rtheta = Math.PI * theta / 180;
		double dist =
			Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
			Math.Cos(rlat2) * Math.Cos(rtheta);
		dist = Math.Acos(dist);
		dist = dist * 180 / Math.PI;
		dist = dist * 60 * 1.1515;

		//Ensure calculations return positive values
		dist = dist < 0 ? (dist * -1) : dist;

		//Convert dist to kilometers
		return dist * 1.609344;
	}
}