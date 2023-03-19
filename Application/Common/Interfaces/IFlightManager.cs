using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.FlightAggregate;

namespace Application.Common.Interfaces;

public interface IFlightManager
{
    ValueTask<Guid> AddAsync(Flight flight, CancellationToken cancellationToken = default);
    ValueTask UpdateAsync(Flight flight, CancellationToken cancellationToken = default);
    ValueTask RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}
