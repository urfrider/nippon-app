namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels;

//NOTE:
//	Objects within this file adheres to JSON format.
//	Properties of objects within this file follows camel case
//	in compliance to JSON format.

public class FeatureCollection {
	public string type { get; set; } = "FeatureCollection";
	public Feature[]? features { get; set; }
}

public class Feature {
	public string type { get; set; } = "Feature";
	public FeatureProperties? properties { get; set; }
	public FeatureGeometry? geometry { get; set; }

	public Feature() { }
	public Feature(FeatureProperties inputProperties, FeatureGeometry inputGeometry) {
		properties = inputProperties;
		geometry = inputGeometry;
	}
}

public class FeatureProperties {
	public int dbh { get; set; } = 0;

	public FeatureProperties() { }
	public FeatureProperties(int inputDbh) {
		dbh = inputDbh;
	}
}

public class FeatureGeometry {
	public string type { get; set; } = "Point";
	public decimal[] coordinates { get; set; } = new decimal[] { 0.0M, 0.0M };

	public FeatureGeometry() { }
	public FeatureGeometry(decimal inputX, decimal inputY) {
		coordinates = new decimal[] { inputX, inputY };
	}
}