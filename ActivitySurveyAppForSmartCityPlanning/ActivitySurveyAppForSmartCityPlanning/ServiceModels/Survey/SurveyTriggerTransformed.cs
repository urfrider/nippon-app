using ActivitySurveyAppForSmartCityPlanning.Services;

namespace ActivitySurveyAppForSmartCityPlanning.ServiceModels;

public class SurveyTriggerData {
	public bool IsTriggered { get; set; }
	public Guid TriggeredSurveyId { get; set; }
	public Guid TriggeredSurveyTriggerId { get; set; }

	public SurveyTriggerData() {
		this.IsTriggered = false;
	}
	public SurveyTriggerData(
		bool inputIsTriggered,
		Guid inputTriggeredSurveyId,
		Guid inputTriggeredSurveyTriggerId) {
		this.IsTriggered = inputIsTriggered;
		this.TriggeredSurveyId = inputTriggeredSurveyId;
		this.TriggeredSurveyTriggerId = inputTriggeredSurveyTriggerId;
	}
}

public class SurveyTriggerSet {
	private class SurveyTriggerTransformed {
		public double Longitude { get; set; }
		public double Latitude { get; set; }
		public double Range { get; set; }

		public Guid? SurveyId { get; set; }
		public Guid SurveyTriggerId { get; set; }

		public SurveyTriggerTransformed(
			double inputLong,
			double inputLat,
			int inputRange,
			Guid? inputSurveyId,
			Guid inputSurveyTriggerId) {
			this.Longitude = inputLong;
			this.Latitude = inputLat;
			this.Range = inputRange;
			this.SurveyId = inputSurveyId;
			this.SurveyTriggerId = inputSurveyTriggerId;
		}
	}

	private List<SurveyTriggerTransformed> Triggers = new List<SurveyTriggerTransformed>();

	public SurveyTriggerSet(
		double inputLong,
		double inputLat,
		int inputRange,
		Guid? inputSurveyId,
		Guid inputSurveyTriggerId
		) {
		this.AddTrigger(
			inputLong,
			inputLat,
			inputRange,
			inputSurveyId,
			inputSurveyTriggerId);
	}

	public void AddTrigger(
		double inputLong,
		double inputLat,
		int inputRange,
		Guid? inputSurveyId,
		Guid inputSurveyTriggerId) {
		//Trim input coordinates to meter(m) precision
		inputLong = Math.Truncate(1000000 * inputLong) / 1000000;
		inputLat = Math.Truncate(1000000 * inputLat) / 1000000;

		//Append as trigger
		this.Triggers.Add(
			new SurveyTriggerTransformed(
				inputLong,
				inputLat,
				inputRange,
				inputSurveyId,
				inputSurveyTriggerId
				));
	}

	public SurveyTriggerData IsTriggered(double inputLong, double inputLat) {
		HelperService helper = new HelperService();
		double distance;

		try {
			foreach (SurveyTriggerTransformed trigger in this.Triggers) {
				distance = helper.CalculateDistBtwCoords(
					trigger.Latitude,
					trigger.Longitude,
					inputLat,
					inputLong);

				//Check if distance is within trigger condition
				if (distance <= trigger.Range) {
					return new SurveyTriggerData(
						true,
						trigger.SurveyId ?? Guid.Empty,
						trigger.SurveyTriggerId);
				}
			}
			return new SurveyTriggerData();
		}
		catch {
			return new SurveyTriggerData();
		}
	}
}