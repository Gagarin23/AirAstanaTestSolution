using Api.Filters;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
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
                .WriteTo.File("Logs/migration.log")
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
            
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddApplication();

            services.ConfigureSettings(_configuration);

            services.AddInfrastructure(_configuration);

            services.AddCors();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen
            (
                options =>
                {
                    options.SwaggerDoc
                    (
                        "v1",
                        new OpenApiInfo
                        {
                            Version = "v1",
                            Title = "API",
                            Description = "An ASP.NET Core Web API",
                        }
                    );

                    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                }
            );

            services.AddOptions();
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
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}