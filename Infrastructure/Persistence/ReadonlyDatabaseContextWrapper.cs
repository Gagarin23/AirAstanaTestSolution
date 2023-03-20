using System.Linq;
using Infrastructure.DbEntities;
using Infrastructure.Interfaces;

namespace Infrastructure.Persistence;

//в дальнейшем заменить на контекст с коннектом к slave readonly репликам
public class ReadonlyDatabaseContextWrapper : IReadonlyDatabaseContext
{
    private readonly IDatabaseContext _context;

    public ReadonlyDatabaseContextWrapper(IDatabaseContext context)
    {
        _context = context;
    }

    public IQueryable<FlightDbModel> Flights => _context.Flights;
    public IQueryable<FlightStatusDbModel> FlightStatuses => _context.FlightStatuses;
}
