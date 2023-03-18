using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DatabaseMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/migration.log", rollingInterval: RollingInterval.Hour)
                .CreateLogger();

            try
            {
                Log.Information("Starting database migration...");

                MigrateDatabase();

                Log.Information("Database migration completed.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An error occurred while migrating the database.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void MigrateDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.LogTo(Log.Logger.Information, LogLevel.Information);
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString"));

            using (var dbContext = new DatabaseContext(optionsBuilder.Options))
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
