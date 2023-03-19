using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Interfaces
{
    public interface IDatabaseContext
    {
        DbSet<FlightDbModel> Flights { get; }
        DbSet<FlightStatusDbModel> FlightStatuses { get; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
