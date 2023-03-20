using System.Linq;
using Infrastructure.DbEntities;

namespace Infrastructure.Interfaces;

public interface IReadonlyDatabaseContext
{
    IQueryable<FlightDbModel> Flights { get; }
    IQueryable<FlightStatusDbModel> FlightStatuses { get; }
}
