using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.FlightAggregate;
using Domain.Interfaces;
using Infrastructure.DbEntities;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM.Contracts;

namespace Infrastructure.Persistence
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public virtual DbSet<FlightDbModel> Flights { get; set; }
        public virtual DbSet<FlightStatusDbModel> FlightStatuses { get; set; }

        public IServiceProvider AsServiceProvider() => ((IInfrastructure<IServiceProvider>)this).Instance;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var provider = AsServiceProvider();

            ExtractEntriesPerType(out var aggregates);

            await PublishNotificationsAsync(provider, aggregates, cancellationToken);

            var affectedRows = await base.SaveChangesAsync(cancellationToken);

            return affectedRows;
        }

        private async ValueTask PublishNotificationsAsync(IServiceProvider provider, IReadOnlyCollection<IAggregate> aggregates, CancellationToken cancellationToken)
        {
            if (aggregates.Count < 1)
            {
                return;
            }
            
            var mediator = provider.GetRequiredService<IMediator>();

            foreach (var notification in aggregates.SelectMany(aggregate => aggregate.Notifications))
            {
                await mediator.Publish(notification, cancellationToken);
            }
        }
        
        private void ExtractEntriesPerType(out List<IAggregate> aggregates)
        {
            aggregates = new List<IAggregate>();

            //если понадобиться выделить другие коллекции,
            //то обобщённая версия Entries<T> не подойдёт.
            foreach (var entry in ChangeTracker.Entries())
            {
                var entity = entry.Entity;

                switch (entity)
                {
                    case IAggregate aggregate:
                        aggregates.Add(aggregate);
                        break;
                }
            }
        }
    }
}
