using CsvHelper.Configuration;

namespace ActivitySurveyAppForSmartCityPlanning.Models.Gps {
	public sealed class GpsCsv : ClassMap<GpsLog> {
		//columns that will be generated in csv
		public GpsCsv() {
			Map(m => m.GpsId);
			Map(m => m.Latitude);
			Map(m => m.Longitude);
			Map(m => m.Accuracy);
			Map(m => m.Altitude);
			Map(m => m.AltitudeAccuracy);
			Map(m => m.Heading);
			Map(m => m.Speed);
			Map(m => m.GpsTimestamp);
			Map(m => m.DeletedAt);
			Map(m => m.DeletedBy);
			Map(m => m.CreatedAt);
			Map(m => m.CreatedBy);
			Map(m => m.ModifiedAt);
			Map(m => m.ModifiedBy);
			Map(m => m.AccId);
		}
	}
}