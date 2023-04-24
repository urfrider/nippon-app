namespace SmartSurveyTests;
using ActivitySurveyAppForSmartCityPlanning;

using System.Security.Claims;
using ActivitySurveyAppForSmartCityPlanning.Controllers.Login;
using ActivitySurveyAppForSmartCityPlanning.Models;
using ActivitySurveyAppForSmartCityPlanning.ServiceModels;
using ActvitySurveyAppForSmartCityPlanning.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ActivitySurveyAppForSmartCityPlanning.Controllers;

public class UnitTestInitialTest
{

    // For checking if Test System is working.

    [Fact]
    public void initialSampleTestRunner()
    {
        string result = "resultTest";

        Assert.Equal("resultTest", result);
    }

}

public class RegistrationControllerTests
{

    // Unit Test to check if registration is successful
    [Fact]
    public void RegisterMobileUser_WithValidInput_ReturnsOk()
    {
        var appSettings = Options.Create(new AppSettings { AccountConfig = new AccountConfig { AccountPasswordSalt = "salty" } });
        var dbContextOptions = new DbContextOptionsBuilder<TravelRewardsContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var dbContext = new TravelRewardsContext(dbContextOptions);
        var controller = new RegistrationController(appSettings, dbContext);

        var registration = new RegisterAccount
        {
            Username = "UNITTEST_USER",
            Password = "testpassword",
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

        // Act
        var result = controller.RegisterMobileUser(registration);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User UNITTEST_USER Created. Role: 0", okResult.Value);

        dbContext.Database.EnsureDeleted();
    }

    // Unit Test to check existing user exists.
    [Fact]
    public void RegisterMobileUser_WithValidInput_ReturnsUserExisted()
    {
        var appSettings = Options.Create(new AppSettings { AccountConfig = new AccountConfig { AccountPasswordSalt = "salty" } });
        var dbContextOptions = new DbContextOptionsBuilder<TravelRewardsContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var dbContext = new TravelRewardsContext(dbContextOptions);
        var controller = new RegistrationController(appSettings, dbContext);

        var registration = new RegisterAccount
        {
            Username = "UNITTEST_USER",
            Password = "testpassword",
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

        // Act
        var result = controller.RegisterMobileUser(registration);

        //Register again with the same exact details
        var result2 = controller.RegisterMobileUser(registration);

        // Assert
        var usernameExistsResults = Assert.IsType<ObjectResult>(result2);
        var errorResponse = Assert.IsType<string>(usernameExistsResults.Value);
        Assert.Equal("8: Username Existed.", errorResponse);

        dbContext.Database.EnsureDeleted();
    }

    // Unit Test to check existing user exists.
    [Fact]
    public void RegisterMobileUser_WithValidInput_Returns400BadRequest()
    {
        var appSettings = Options.Create(new AppSettings { AccountConfig = new AccountConfig { AccountPasswordSalt = "salty" } });
        var dbContextOptions = new DbContextOptionsBuilder<TravelRewardsContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var dbContext = new TravelRewardsContext(dbContextOptions);
        var controller = new RegistrationController(appSettings, dbContext);

        // Since the rest of the data is not a required and handled by frontend, the backend ensures that the minimum field is username and password
        var registration = new RegisterAccount
        {
            Username = "",
            Password = "",
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

        // Act
        var result = controller.RegisterMobileUser(registration);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var errorResponse = Assert.IsType<string>(objectResult.Value);

        Assert.Equal("Malformed body content detected", objectResult.Value);
        dbContext.Database.EnsureDeleted();
    }

}