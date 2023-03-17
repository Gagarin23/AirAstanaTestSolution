using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Application.Common.Interfaces
{
    public interface IDatabaseContext
    {
        public DatabaseFacade Database { get; }
        public ChangeTracker ChangeTracker { get; }
        public IModel Model { get; }

        IServiceProvider AsServiceProvider();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
