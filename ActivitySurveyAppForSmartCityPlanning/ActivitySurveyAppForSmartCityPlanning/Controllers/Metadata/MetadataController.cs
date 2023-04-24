using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.Models.Gps;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels.Metadata;
using ActivitySurveyAppForSmartCityPlanning.Services;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ActivitySurveyAppForSmartCityPlanning.Controllers;

[ApiController, Route("api/[controller]"), EnableCors("CORSPolicy")]
public class MetadataController : Controller {
	private TravelRewardsContext _dbContext { get; set; }

	#region Class Constructor(s)
	public MetadataController(TravelRewardsContext dbContextInjection) {
		_dbContext = dbContextInjection;
	}
	#endregion Class Constructor(s)

	#region API Method(s)
	//Counting total number of surveys
	[HttpGet, Route("TotalSurvey"), Authorize(Roles = "DashboardUser")]
	public int GetTotalSurvey() {
		try {
			int count = _dbContext.Surveys.Where(x => String.Equals(x.DeletedAt, null)).Count();
			return count;
		}
		catch (DbUpdateException) {
			return -1;
		}
		catch (Exception) {
			return -2;
		}
	}

	//Counting total number of responses
	[HttpGet, Route("TotalResponse"), Authorize(Roles = "DashboardUser")]
	public int GetTotalResponse() {
		try {
			int count = _dbContext.Responses.Where(x => String.Equals(x.DeletedAt, null)).Count();
			return count;
		}
		catch (DbUpdateException) {
			return -1;
		}
		catch (Exception) {
			return -2;
		}
	}

	//Counting total number of accounts
	[HttpGet, Route("TotalAccount"), Authorize(Roles = "DashboardUser")]
	public int GetTotalAcc() {
		try {
			int count = _dbContext.Accounts.Where(x => String.Equals(x.DeletedAt, null)).Count();
			return count;
		}
		catch (DbUpdateException) {
			return -1;
		}
		catch (Exception) {
			return -2;
		}
	}

	//Counting total number of gps logs
	[HttpGet, Route("TotalGps"), Authorize(Roles = "DashboardUser")]
	public int GetTotalGps() {
		try {
			int count = _dbContext.GpsLogs.Where(x => String.Equals(x.DeletedAt, null)).Count();
			return count;
		}
		catch (DbUpdateException) {
			return -1;
		}
		catch (Exception) {
			return -2;
		}
	}

	//Retrieving gps logs based on time period
	[HttpGet, Route("TotalGpsOneMonth"), Authorize(Roles = "DashboardUser")]
	public List<GpsLog>? GetOneMonthGps() {
		DateTime now = DateTime.Now;
		DateTime onemonthPeriod = DateTime.Now.AddDays(-30);

		try {
			List<GpsLog>? selectedLogs = (List<GpsLog>?)_dbContext.GpsLogs
				.Where(x => x.GpsTimestamp >= onemonthPeriod)
				.ToList();

			return selectedLogs;
		}
		catch {
			throw;
		}
	}

	[HttpGet, Route("TotalGpsTwoMonth"), Authorize(Roles = "DashboardUser")]
	public List<GpsLog>? GetTwoMonthGps() {
		DateTime now = DateTime.Now;
		DateTime twomonthPeriod = DateTime.Now.AddDays(-60);

		try {
			List<GpsLog>? selectedLogs = (List<GpsLog>?)_dbContext.GpsLogs
				.Where(x => x.GpsTimestamp >= twomonthPeriod)
				.ToList();

			return selectedLogs;
		}
		catch {
			throw;
		}
	}

	[HttpGet, Route("GetGPSDataChart"), Authorize]
	public List<MetadataGPSCount>? GPSDataChart() {
		try {
			List<MetadataGPSCount> returnList = new List<MetadataGPSCount>();

			var selectedLogs = _dbContext.GpsLogs
				.GroupBy(g => new { Year = g.CreatedAt.Value.Year, Month = g.CreatedAt.Value.Month })
				.Select(group => new {
					Year = group.Key.Year,
					Month = group.Key.Month,
					Count = group.Count()
				})
				.OrderBy(result => result.Year)
				.ThenBy(result => result.Month)
				.ToList();

			foreach (var gpslogs in selectedLogs) {
				MetadataGPSCount gpscount = new MetadataGPSCount();
				gpscount.Month = gpslogs.Month;
				gpscount.Year = gpslogs.Year;
				gpscount.Count = gpslogs.Count;

				returnList.Add(gpscount);
			}

			return returnList;
		}
		catch {
			throw;
		}
	}

	[HttpGet, Route("GetGPSDataChart/{id}"), Authorize]
	public List<MetadataGPSCount>? GPSDataChartIndividual(String id) {
		try {
			List<MetadataGPSCount> returnList = new List<MetadataGPSCount>();

			var selectedLogs = _dbContext.GpsLogs
				.Where(w => w.AccId.Value.ToString() == id)
				.GroupBy(g => new { Year = g.CreatedAt.Value.Year, Month = g.CreatedAt.Value.Month })
				.Select(group => new {
					Year = group.Key.Year,
					Month = group.Key.Month,
					Count = group.Count()
				})
				.OrderBy(result => result.Year)
				.ThenBy(result => result.Month)
				.ToList();

			foreach (var gpslogs in selectedLogs) {
				MetadataGPSCount gpscount = new MetadataGPSCount();
				gpscount.Month = gpslogs.Month;
				gpscount.Year = gpslogs.Year;
				gpscount.Count = gpslogs.Count;

				returnList.Add(gpscount);
			}

			return returnList;
		}
		catch {
			throw;
		}
	}

	[HttpGet, Route("GetPublicTransportCount"), Authorize]
	public List<MetadataPublicTransportCount>? GPSPublicTransportChart() {
		try {
			List<MetadataPublicTransportCount> returnList = new List<MetadataPublicTransportCount>();

			var selectedTransport = _dbContext.Responses
				.GroupBy(r => r.ByPublicTransport)
				.Select(g => new { PublicTransport = g.Key, Quantity = g.Count() })
				.ToList();

			foreach (var transport in selectedTransport) {
				MetadataPublicTransportCount transportCount = new MetadataPublicTransportCount();
				transportCount.PublicTransport = transport.PublicTransport;
				transportCount.Quantity = transport.Quantity;
				returnList.Add(transportCount);
			}

			return returnList;
		}
		catch {
			throw;
		}
	}

	//Counting total number of responses in months
	[HttpGet, Route("GetResponsesDataChart"), Authorize(Roles = "DashboardUser")]
	public List<MetadataResponsesCount>? ResponsesDataChart() {
		try {
			List<MetadataResponsesCount> returnList = new List<MetadataResponsesCount>();

			var selectedResponses = _dbContext.Responses
				.GroupBy(g => new { Year = g.CreatedAt.Value.Year, Month = g.CreatedAt.Value.Month })
				.Select(group => new {
					Year = group.Key.Year,
					Month = group.Key.Month,
					Count = group.Count()
				})
				.OrderBy(result => result.Year)
				.ThenBy(result => result.Month)
				.ToList();

			foreach (var responses in selectedResponses) {
				MetadataResponsesCount responsecount = new MetadataResponsesCount();
				responsecount.Month = responses.Month;
				responsecount.Year = responses.Year;
				responsecount.Count = responses.Count;

				returnList.Add(responsecount);
			}

			return returnList;
		}
		catch {
			throw;
		}
	}

	[HttpGet, Route("GetResponseData/{inputSurveyId}"), AllowAnonymous]
	public async Task<IActionResult> GetResponseData(string inputSurveyId) {
		#region Initialize Variable(s)
		Survey? targetSurvey;
		SurveyQuestion[] targetSurveyQuestions;
		ResponseQuestion[] targetResponseQuestions;
		List<MetadataQnsResData> returnData = new List<MetadataQnsResData>();
		#endregion Initialize Variable(s)

		#region Acquire Target Survey
		targetSurvey = await _dbContext.Surveys
			.Where(x => String.Equals(x.SurveyId.ToString(), inputSurveyId))
			.FirstOrDefaultAsync();
		if (targetSurvey == null) return StatusCode(400);
		#endregion Acquire Target Survey

		#region Acquire Target Survey Questions
		targetSurveyQuestions = await _dbContext.SurveyQuestions
			.Where(x => String.Equals(x.SurveyId.ToString(), inputSurveyId))
			.ToArrayAsync();
		#endregion Acquire Target Survey Questions

		foreach (SurveyQuestion targetSurveyQuestion in targetSurveyQuestions) {
			try {
				switch (targetSurveyQuestion.QnsType) {
					//Open Ended
					case 0:
						string[] openEnded = await _dbContext.ResponseQuestions
							.Where(x => x.QnsId == targetSurveyQuestion.QnsId)
							.Select(x => x.ResQnsString ?? "")
							.ToArrayAsync();

						MetadataQnsResData oeReturnData = new MetadataQnsResData();
						oeReturnData.Qns = targetSurveyQuestion.Qns;
						oeReturnData.QnsType = 0;
						oeReturnData.TotalRes = openEnded.Length;

						oeReturnData.opendEndedData = openEnded;

						returnData.Add(oeReturnData);
						break;

					//Single Choice & Likert
					case 1:
					case 3:
						targetResponseQuestions = await _dbContext.ResponseQuestions
							.Where(x => x.QnsId == targetSurveyQuestion.QnsId)
							.ToArrayAsync();

						//Very ugly, yes I know
						List<string> scLabelsList = new List<string>();
						if (targetSurveyQuestion.QnsOption01 != null) scLabelsList.Add(targetSurveyQuestion.QnsOption01);
						if (targetSurveyQuestion.QnsOption02 != null) scLabelsList.Add(targetSurveyQuestion.QnsOption02);
						if (targetSurveyQuestion.QnsOption03 != null) scLabelsList.Add(targetSurveyQuestion.QnsOption03);
						if (targetSurveyQuestion.QnsOption04 != null) scLabelsList.Add(targetSurveyQuestion.QnsOption04);
						if (targetSurveyQuestion.QnsOption05 != null) scLabelsList.Add(targetSurveyQuestion.QnsOption05);
						if (targetSurveyQuestion.QnsOption06 != null) scLabelsList.Add(targetSurveyQuestion.QnsOption06);
						if (targetSurveyQuestion.QnsOption07 != null) scLabelsList.Add(targetSurveyQuestion.QnsOption07);
						if (targetSurveyQuestion.QnsOption08 != null) scLabelsList.Add(targetSurveyQuestion.QnsOption08);

						int[] scData = new int[scLabelsList.Count];
						Array.Clear(scData, 0, scData.Length);

						foreach (ResponseQuestion targetResponseQuestion in targetResponseQuestions) {
							try {
								scData[targetResponseQuestion.ResQnsInt ?? 0]++;
							}
							catch (IndexOutOfRangeException) {
								//Ignore
							}
						}

						MetadataQnsResData scReturnData = new MetadataQnsResData();
						scReturnData.Qns = targetSurveyQuestion.Qns;
						scReturnData.QnsType = 1;
						scReturnData.TotalRes = targetResponseQuestions.Length;

						scReturnData.labels = scLabelsList.ToArray();
						scReturnData.data = scData;

						returnData.Add(scReturnData);
						break;

					//Multi Choice
					case 2:
						targetResponseQuestions = await _dbContext.ResponseQuestions
							.Where(x => x.QnsId == targetSurveyQuestion.QnsId)
							.ToArrayAsync();

						//Very ugly, yes I know
						List<string> mclabelsList = new List<string>();
						if (targetSurveyQuestion.QnsOption01 != null) mclabelsList.Add(targetSurveyQuestion.QnsOption01);
						if (targetSurveyQuestion.QnsOption02 != null) mclabelsList.Add(targetSurveyQuestion.QnsOption02);
						if (targetSurveyQuestion.QnsOption03 != null) mclabelsList.Add(targetSurveyQuestion.QnsOption03);
						if (targetSurveyQuestion.QnsOption04 != null) mclabelsList.Add(targetSurveyQuestion.QnsOption04);
						if (targetSurveyQuestion.QnsOption05 != null) mclabelsList.Add(targetSurveyQuestion.QnsOption05);
						if (targetSurveyQuestion.QnsOption06 != null) mclabelsList.Add(targetSurveyQuestion.QnsOption06);
						if (targetSurveyQuestion.QnsOption07 != null) mclabelsList.Add(targetSurveyQuestion.QnsOption07);
						if (targetSurveyQuestion.QnsOption08 != null) mclabelsList.Add(targetSurveyQuestion.QnsOption08);

						int[] mcData = new int[mclabelsList.Count];
						Array.Clear(mcData, 0, mcData.Length);

						foreach (ResponseQuestion targetResponseQuestion in targetResponseQuestions) {
							try {
								if (targetResponseQuestion.ResQnsString == null) continue;

								string[] inputValues = targetResponseQuestion.ResQnsString.Split(",");
								foreach (string inputValue in inputValues) {
									mcData[Int32.Parse(inputValue)]++;
								}
							}
							catch (IndexOutOfRangeException) {
								//Ignore count
							}
						}

						MetadataQnsResData mcReturnData = new MetadataQnsResData();
						mcReturnData.Qns = targetSurveyQuestion.Qns;
						mcReturnData.QnsType = 2;
						mcReturnData.TotalRes = targetResponseQuestions.Length;

						mcReturnData.labels = mclabelsList.ToArray();
						mcReturnData.data = mcData;

						returnData.Add(mcReturnData);
						break;

					default:
						break;
				}
			}
			catch {
				//Ignore question
			}
		}
		return Ok(returnData);
	}

	[HttpGet, Route("ExportAllGps"), AllowAnonymous]
	public async Task<IActionResult> ExportGps() {
		try {
			//Retrieve GPS logs from database
			List<GpsLog> allGpsLogs = await _dbContext.GpsLogs
				.OrderByDescending(x => x.GpsTimestamp)
				.ToListAsync();

			//Generate csv file name
			string fileId = Guid.NewGuid().ToString("N");
			string timeStamp = DateTime.UtcNow.Date.ToString("dd-MM-yyyy");
			string genFileName = timeStamp + "_" + fileId;

			//Garbage cleanup
			string generationDir = "GeneratedFiles";
			List<string> flaggedFiles = new List<string>();
			if (Directory.Exists(generationDir)) {
				string[] files = Directory.GetFiles(generationDir);
				foreach (string file in files) {
					string fileName = file;
					while (fileName.Contains("/") || fileName.Contains("\\")) {
						if (fileName.Contains("/"))
							fileName = file.Split("/")[(file.Split("/").Length - 1)];
						if (fileName.Contains("\\"))
							fileName = file.Split("\\")[(file.Split("\\").Length - 1)];
					}

					if (!fileName.Contains("#")) {
						string fileDatestamp = fileName.Split("_")[0];
						DateTime fileTsDate = DateTime.ParseExact(fileDatestamp, "dd-MM-yyyy", null);

						if (fileTsDate < (DateTime.Today).AddDays(-1))
							flaggedFiles.Add(file);
					}
				}

				foreach (string flaggedFile in flaggedFiles) {
					if (System.IO.File.Exists(flaggedFile)) {
						System.IO.File.Delete(flaggedFile);
					}
				}
			}
			else {
				Directory.CreateDirectory(generationDir);
			}

			//Generate csv file
			using (var writer = new StreamWriter(generationDir + "/" + genFileName + ".csv")) {
				//Csv helper package to write into csv file
				using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
					csv.Context.RegisterClassMap<GpsCsv>();
					await csv.WriteRecordsAsync(allGpsLogs);
				}
			}

			Stream fileStream = System.IO.File.OpenRead(generationDir + "/" + genFileName + ".csv");

			return File(fileStream, "text/csv", genFileName + ".csv");
		}
		catch (DbUpdateException ex) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(1) + ex.Message.ToString());
		}
		catch (Exception e) {
			return StatusCode(500, new ErrorExceptionHelper().Exception500(0) + e.Message.ToString());
		}
	}
	#endregion API Method(s)
}