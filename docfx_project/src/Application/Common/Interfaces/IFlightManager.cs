using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.FlightAggregate;

namespace Application.Common.Interfaces;

/// <summary>
/// Контракт для изменения состояния агрегата рейсов
/// </summary>
public interface IFlightManager
{
    ValueTask<Guid> AddAsync(Flight flight, CancellationToken cancellationToken = default);
    ValueTask UpdateAsync(Flight flight, CancellationToken cancellationToken = default);
    ValueTask RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}
