using System;
using ActivitySurveyAppForSmartCityPlanning;
using ActivitySurveyAppForSmartCityPlanning.Controllers;
using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActivitySurveyAppForSmartCityPlanning.Controllers.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SmartSurveyTests
{
    public class UnitTestLogin
    {
        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            var appSettings = Options.Create(new AppSettings { AccountConfig = new AccountConfig { AccountPasswordSalt = "veryverysalty" } });
            var dbContextOptions = new DbContextOptionsBuilder<TravelRewardsContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new TravelRewardsContext(dbContextOptions);
            var registrationController = new RegistrationController(appSettings, dbContext);
            var loginController = new LoginController(appSettings, dbContext);
            var tokenService = new TokenService(appSettings);
            var username = "UNITTEST_LOGIN_TEST";
            var password = "testpassword";
            var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password + appSettings.Value.AccountConfig.AccountPasswordSalt);

            var registration = new RegisterAccount
            {
                Username = username,
                Password = password,
                FirstName = "Test",
                LastName = "User",
                Gender = 0,
                Age = 30,
                PhoneCountryCode = "+1",
                PhoneNumber = "1234567890",
                Country = "SINGAPORE",
                City = "SINGAPORE",
                ZipCode = "10001",
                StreetAddress = "Nanyang Polytechnic #01-001",
                EmploymentStatus = "Unemployed",
                Occupation = "Student",
                EmploymentLocation = "Singapore",
                EmploymentStartTime = 2020,
                EmploymentEndTime = 2022,
                EmploymentAnnualSalary = 0,
                DriverLicense = "123456789",
                MobilityImpaired = 0,
                HouseholdPosition = "Head",
                NoOfVehicles = 1
            };

            // Register a user in-memory first
            var result = registrationController.RegisterMobileUser(registration);

            // Attempt to Login

            var LoginTest = new LoginCredentials
            {
                Username = username,
                Password = password
            };

            var results = await loginController.Login(LoginTest);

            var okResults = Assert.IsType<OkObjectResult>(result);
            var token = Assert.IsType<string>(okResults.Value);
            Assert.Equal(200, okResults.StatusCode);
            Assert.NotEmpty(token);

            dbContext.Database.EnsureDeleted();
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsInvalidLogin()
        {
            var appSettings = Options.Create(new AppSettings { AccountConfig = new AccountConfig { AccountPasswordSalt = "veryverysalty" } });
            var dbContextOptions = new DbContextOptionsBuilder<TravelRewardsContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new TravelRewardsContext(dbContextOptions);
            var registrationController = new RegistrationController(appSettings, dbContext);
            var loginController = new LoginController(appSettings, dbContext);
            var tokenService = new TokenService(appSettings);
            var username = "UNITTEST_LOGIN_TEST";
            var password = "testpassword";
            var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password + appSettings.Value.AccountConfig.AccountPasswordSalt);

            var registration = new RegisterAccount
            {
                Username = username,
                Password = password,
                FirstName = "Test",
                LastName = "User",
                Gender = 0,
                Age = 30,
                PhoneCountryCode = "+1",
                PhoneNumber = "1234567890",
                Country = "SINGAPORE",
                City = "SINGAPORE",
                ZipCode = "10001",
                StreetAddress = "Nanyang Polytechnic #01-001",
                EmploymentStatus = "Unemployed",
                Occupation = "Student",
                EmploymentLocation = "Singapore",
                EmploymentStartTime = 2020,
                EmploymentEndTime = 2022,
                EmploymentAnnualSalary = 0,
                DriverLicense = "123456789",
                MobilityImpaired = 0,
                HouseholdPosition = "Head",
                NoOfVehicles = 1
            };

            // Register a user in-memory first
            var result = registrationController.RegisterMobileUser(registration);

            // Attempt to Login

            var LoginTest = new LoginCredentials
            {
                Username = username,
                Password = "wrongpassword"
            };

            var results = await loginController.Login(LoginTest);

            var badLoginResults = Assert.IsType<ObjectResult>(results);

            Assert.Equal(401, badLoginResults.StatusCode);
            Assert.Equal("6: Invalid Login.", badLoginResults.Value);
            dbContext.Database.EnsureDeleted();

        }

        // Fake token services
        public interface ITokenService
        {
            string GenerateJwt(Account account);
        }

        public class TokenService : ITokenService
        {
            private readonly AppSettings _appSettings;

            public TokenService(IOptions<AppSettings> appSettings)
            {
                _appSettings = appSettings.Value;
            }

            public string GenerateJwt(Account account)
            {
                return "very_real_JWT";
            }
        }
    }
}

