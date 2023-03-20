using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.FlightAggregate;
using Domain.Interfaces;
using Infrastructure.DbEntities;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redis.OM.Contracts;

namespace Infrastructure.Persistence
{
    public class DatabaseContext : IdentityDbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public virtual DbSet<FlightDbModel> Flights { get; set; }
        public virtual DbSet<FlightStatusDbModel> FlightStatuses { get; set; }

        public IServiceProvider AsServiceProvider() => ((IInfrastructure<IServiceProvider>)this).Instance;

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(true, cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var provider = AsServiceProvider();
            var logger = provider.GetRequiredService<ILogger<DatabaseContext>>();
            var userContext = provider.GetRequiredService<IUserContext>();
            
            var changes = ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached)
                .Select
                (
                    e => new
                    {
                        Entity = e.Entity,
                        State = e.State.ToString("G")
                    }
                )
                .ToList();

            try
            {
                var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

                logger.LogInformation
                (
                    "Пользовать: {Username} отправил запрос на изменение данных:\n {Changes}",
                    userContext.Username,
                    JsonSerializer.Serialize(changes)
                );

                return result;
            }
            catch(Exception ex)
            {
                logger.LogError
                (
                    ex,
                    "Пользовать: {Username} неудачная попытка изменить данные:\n {Changes}",
                    userContext.Username,
                    JsonSerializer.Serialize(changes)
                );

                throw;
            }
        }
    }
}
