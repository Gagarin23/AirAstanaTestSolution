using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.FlightAggregate;
using Domain.Interfaces;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Flight> Flights { get; set; }

        public IServiceProvider AsServiceProvider() => ((IInfrastructure<IServiceProvider>)this).Instance;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var provider = AsServiceProvider();

            IdentifyEntries(out var cacheables, out var aggregates);

            await PublishNotificationsAsync(provider, aggregates, cancellationToken);

            var affectedRows = await base.SaveChangesAsync(cancellationToken);
            
            await ActualizeCacheAsync(provider, cacheables, cancellationToken);

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

        private Task ActualizeCacheAsync(IServiceProvider provider, IReadOnlyCollection<ICacheable> cacheables, CancellationToken cancellationToken)
        {
            if (cacheables.Count < 1)
            {
                return Task.CompletedTask;
            }
            
            var cache = provider.GetRequiredService<IDistributedCache>();

            return Task.WhenAll
            (
                cacheables.Select(cacheable => cache.SetAsync(cacheable, cancellationToken))
            );
        }

        private void IdentifyEntries(out List<ICacheable> cacheables, out List<IAggregate> aggregates)
        {
            cacheables = new List<ICacheable>();
            aggregates = new List<IAggregate>();

            foreach (var entry in ChangeTracker.Entries())
            {
                var entity = entry.Entity;

                if (entity is ICacheable cacheable)
                {
                    cacheables.Add(cacheable);
                }

                if (entity is IAggregate aggregate)
                {
                    aggregates.Add(aggregate);
                }
            }
        }
    }
}
