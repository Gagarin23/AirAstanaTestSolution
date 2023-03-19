using Api.Filters;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using Api.Configs;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;
using Serilog.Events;

namespace Api
{
    public static class Program
    {
        private static ConfigurationManager _configuration;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            BuildConfiguration(builder.Configuration);
            CreateLogger(builder.Logging);
            ConfigureServices(builder.Services);

            var app = builder.Build();
            ConfigureApplication(app);

            app.Run();
        }

        private static void BuildConfiguration(ConfigurationManager configuration)
        {
            configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile
                (
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: true
                )
                .AddJsonFile
                (
                    $"appsettings.{EnvironmentExtension.CurrentEnvironment}.json",
                    optional: true,
                    reloadOnChange: true
                )
                .Build();

            _configuration = configuration;
        }

        private static void CreateLogger(ILoggingBuilder loggingBuilder)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/api.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            loggingBuilder.AddSerilog(logger);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers
                (
                    options =>
                        options.Filters.Add(typeof(ApiExceptionFilter))
                )
                .AddJsonOptions
                (
                    options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                        options.JsonSerializerOptions.WriteIndented = true;
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    }
                );
            
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddEndpointsApiExplorer();

            AddVersioning(services);

            AddIdentity(services);

            services.AddApplication();

            services.AddInfrastructure(_configuration);

            services.AddCors();

            services.AddEndpointsApiExplorer();

            AddSwagger(services);

            services.AddOptions();
        }

        private static void AddVersioning(IServiceCollection services)
        {
            services.AddApiVersioning
            (
                options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                }
            );

            services.AddVersionedApiExplorer
            (
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                }
            );
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.ConfigureOptions<SwaggerGenOptionsConfig>();
            services.ConfigureOptions<SwaggerUIOptionsConfig>();
            
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options => {});
        }

        private static void AddIdentity(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>
                (
                    options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.User.RequireUniqueEmail = false;
                    }
                )
                //для простоты поместил в один контекст
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            //по хорошему нужен сервер аутентификации/авторизации openIdConnect
            //с методом авторизации code flow, с асимитричным алгоритмом подписи, с ротацией сертификатов и тд.
            //для демонстрации упростил.
            //используем симетричное шифрование
            //валидируем только издателя и время жизни
            services.AddAuthentication
                (
                    //перезатираем аутентификацию из identity
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                )
                .AddJwtBearer
                (
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidIssuer = _configuration.GetSection("AuthSettings")["ValidIssuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AuthSettings")["Secret"]))
                        };
                    }
                );

            services.AddAuthorization();
        }

        private static void ConfigureApplication(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors
            (
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }
            );

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
