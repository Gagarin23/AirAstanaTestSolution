using System;
using System.Linq.Expressions;
using System.Reflection;
using Application.Common.Interfaces;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Services.Flights;
using Mapster;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redis.OM;
using Redis.OM.Contracts;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultDatabaseConnection = configuration.GetConnectionString("DefaultConnection");
            
            services.AddEntityFrameworkSqlServer();
            
            services.AddPooledDbContextFactory<DatabaseContext>
            (
                (provider, options) =>
                {
                    options.UseSqlServer
                    (
                        defaultDatabaseConnection,
                        b =>
                            b.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)
                    );
                    
                    options.UseInternalServiceProvider(provider);
                    
                    if (EnvironmentExtension.IsDevelopment)
                    {
                        options.EnableSensitiveDataLogging();
                        options.LogTo(Console.WriteLine, LogLevel.Information);
                    }
                }
            );

            services.AddScoped<IDatabaseContext>
            (
                provider => provider.GetRequiredService<IDbContextFactory<DatabaseContext>>().CreateDbContext()
            );

            //для identity
            services.AddScoped<DatabaseContext>
            (
                provider => provider.GetRequiredService<IDbContextFactory<DatabaseContext>>().CreateDbContext()
            );

            services.AddScoped<IReadonlyDatabaseContext, ReadonlyDatabaseContextWrapper>();
            
            MessagePackSerializer.DefaultOptions = ContractlessStandardResolver.Options;

            services.AddScoped<IFlightManager, FlightManager>();
            services.AddScoped<IReadonlyFlightManager, ReadonlyFlightManager>();
            
            services.AddScoped<IRedisConnectionProvider, RedisConnectionProvider>(_ => new RedisConnectionProvider("redis://localhost:6379"));
            
            TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileWithDebugInfo();
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
