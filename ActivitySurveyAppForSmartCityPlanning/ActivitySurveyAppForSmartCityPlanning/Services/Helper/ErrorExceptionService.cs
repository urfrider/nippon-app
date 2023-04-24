namespace ActivitySurveyAppForSmartCityPlanning.Services;

public class ErrorExceptionHelper {
	public string Exception401(int Selection) {
		switch (Selection) {
			case 0:
				return "0: Malformed token detected.";

			case 1:
				return "1: No Account Id found in token.";

			case 2:
				return "2: Invalid Account Id detected.";

			case 3:
				return "3: Unable to find data in database.";

			case 4:
				return "4: Incorrect Password.";

			case 5:
				return "5: Existing Session Detected.";

			case 6:
				return "6: Invalid Login."; //Either ID or PWD wrong, it will return generic message.

			case 7:
				return "7: No mobile unique identifier detected";

			case 8:
				return "8: Username Existed.";

			case 9:
				return "9: Account not enough points or reward quantity invalid.";
		}
		return "Invalid Exception 401 Selection";
	}
	public string Exception500(int Selection) {
		switch (Selection) {
			case 0:
				return "0:Unexpected Error: ";

			case 1:
				return "1:Database Error: ";
		}
		return "Invalid Use of Exception 500 Message Selection";
	}
}