using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Infrastructure;
using Infrastructure.DbEntities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Redis.OM;
using Redis.OM.Searching;
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

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.LogTo(Log.Logger.Information, LogLevel.Information);
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DbConnectionString"));

                using (var context = new DatabaseContext(optionsBuilder.Options))
                {
                    MigrateDatabase(context);
                    RebuildCache(context);
                }

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

        private static void MigrateDatabase(DatabaseContext context)
        {
            context.Database.Migrate();
        }

        private static void RebuildCache(DatabaseContext context)
        {
            var provider = new RedisConnectionProvider(Environment.GetEnvironmentVariable("RedisConnection"));

            RefreshCache(context, provider);
            CreateIndexes(provider);
        }

        private static void RefreshCache(DatabaseContext context, RedisConnectionProvider provider)
        {
            provider.Connection.Execute("flushall");

            //получаем все объекты имплиментирующие ICacheable
            var cacheableTypes = Assembly.GetAssembly(typeof(InfrastructureAssemblyMark))
                .GetTypes()
                .Where
                (
                    x => x.GetInterfaces()
                        .Any(i => i == typeof(ICacheable))
                );

            var dbContextPropertiesTypes = AccessTools.GetDeclaredProperties(typeof(DatabaseContext));

            var chunkSize = 100;
            
            foreach (var cacheableType in cacheableTypes)
            {
                var targetType = typeof(DbSet<>).MakeGenericType(cacheableType);
                    
                var dbSetPropertyInfo = dbContextPropertiesTypes.First(x => x.PropertyType == targetType);
                var dbSet = dbSetPropertyInfo.GetValue(context);
                    
                var method = AccessTools.Method(typeof(RelationalQueryableExtensions), nameof(RelationalQueryableExtensions.FromSqlRaw));
                
                var query = $"select * from {context.Model.FindEntityType(cacheableType).GetTableName()}";
                //DbSet<>.FromSqlRaw(query)
                var iterator = (IQueryable)method.MakeGenericMethod(cacheableType).Invoke(null, new object?[] { dbSet, query, new object[]{} });
                
                var redisCollection = AccessTools.Method
                    (
                        typeof(RedisConnectionProvider),
                        nameof(RedisConnectionProvider.RedisCollection), 
                        new[] { typeof(int) }
                    )
                    .MakeGenericMethod(cacheableType)
                    .Invoke(provider, new object[] { chunkSize });

                var insertMethod = AccessTools.Method
                (
                    typeof(IRedisCollection<>).MakeGenericType(cacheableType), 
                    nameof(IRedisCollection<object>.Insert), 
                    new[] { cacheableType }
                );
                
                foreach (var obj in iterator)
                {
                    //RedisCollection<>.Insert()
                    insertMethod.Invoke(redisCollection, new[] { obj });
                }
            }
        }

        private static void CreateIndexes(RedisConnectionProvider provider)
        {
            var cacheableTypes = Assembly.GetAssembly(typeof(InfrastructureAssemblyMark))
                .GetTypes()
                .Where
                (
                    x => x.GetInterfaces()
                        .Any(i => i == typeof(ICacheable))
                );

            foreach (var cacheableType in cacheableTypes)
            {
                provider.Connection.CreateIndex(cacheableType);
            }
        }
    }
}
