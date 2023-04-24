using ActivitySurveyAppForSmartCityPlanning;
using ActivitySurveyAppForSmartCityPlanning.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        #region PreBuild Process - Container Services Initialization
        //MVC service
        builder.Services.AddControllersWithViews();

        //Swagger service
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "TravelRewards API Documentations",
                Description = "Welcome to TravelRewards API Documentation!",
            });

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using Bearer scheme (\"bearer {token}\")",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });

        //JWT athentication service
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetSection("AppSettings:JwtConfig:JwtIssuer").Value,
                    ValidateLifetime = true,
                    ValidAudience = builder.Configuration.GetSection("AppSettings:JwtConfig:JwtAudience").Value,
                    ValidateAudience = true,
                    RequireSignedTokens = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        builder.Configuration.GetSection("AppSettings:JwtConfig:JwtTokenKey").Value ?? "default")
                    )
                };
            });

        //Detect system IP address(es)
        List<string> CORSOriginList = new List<string>();
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                CORSOriginList.Add("https://" + ip.ToString() + ":44419");
                CORSOriginList.Add("https://" + ip.ToString() + ":44420");
                CORSOriginList.Add("https://localhost:44419");
                CORSOriginList.Add("https://localhost:44420");
                CORSOriginList.Add("http://localhost:44419");
                CORSOriginList.Add("http://localhost:44420");
                CORSOriginList.Add("http://18.139.118.56:5000");
                CORSOriginList.Add("http://18.139.118.56:5001");
            }
        }

        //Policy to allow CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CORSPolicy",
                policy =>
                {
                    policy.WithOrigins(CORSOriginList.ToArray())
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });

        #endregion PreBuild Process - Container Services Initialization

        #region PreBuild Process - appsettings.json Validation
        //Validate DBConfig settings
        if (
            String.IsNullOrEmpty(builder.Configuration.GetSection("AppSettings:DBConfig:DBEndpoint").Value) ||
            String.IsNullOrEmpty(builder.Configuration.GetSection("AppSettings:DBConfig:DBDatabase").Value) ||
            String.IsNullOrEmpty(builder.Configuration.GetSection("AppSettings:DBConfig:DBUsername").Value) ||
            String.IsNullOrEmpty(builder.Configuration.GetSection("AppSettings:DBConfig:DBPassword").Value)
        )
        {
            throw new Exception("Invalid appsettings.json configuration.");
        }
        #endregion PreBuild Process - appsettings.json Validation

        #region PreBuild Process - Dependency Injection Initialization
        //Configure & bind app settings injection
        builder.Services.AddOptions<AppSettings>()
            .Bind(
                builder.Configuration.GetSection("AppSettings"));

        //Configure & bind DBContext injection
        #region Construct Connection String
        string connectionString = "Server=@_01_@;Database=@_02_@;User Id=@_03_@;Password=@_04_@;TrustServerCertificate=True";

        //Insert parameter: Server
        connectionString = connectionString.Replace("@_01_@", builder.Configuration.GetSection("AppSettings:DBConfig:DBEndpoint").Value);
        //Insert parameter: Database
        connectionString = connectionString.Replace("@_02_@", builder.Configuration.GetSection("AppSettings:DBConfig:DBDatabase").Value);
        //Insert parameter: Username
        connectionString = connectionString.Replace("@_03_@", builder.Configuration.GetSection("AppSettings:DBConfig:DBUsername").Value);
        //Insert parameter: Password
        connectionString = connectionString.Replace("@_04_@", builder.Configuration.GetSection("AppSettings:DBConfig:DBPassword").Value);
        #endregion Construct Connection String
        builder.Services.AddDbContext<TravelRewardsContext>(options =>
            options.UseSqlServer(connectionString)
        );
        #endregion PreBuild Process - Dependency Injection Initialization

        WebApplication app = builder.Build();

        #region PostBuild Process - Middleware Initialization
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });
        }
        else
        {
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");
        #endregion PostBuild Process - Middleware Initialization

        app.Run();
    }
}