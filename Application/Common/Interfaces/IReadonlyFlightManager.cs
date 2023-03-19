using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.FlightAggregate;
using JetBrains.Annotations;

namespace Application.Common.Interfaces;

public interface IReadonlyFlightManager
{
    ValueTask<Flight> GetSingleOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);
    ValueTask<List<Flight>> GetAsync([CanBeNull]IDictionary<string, string> filters, CancellationToken cancellationToken = default);
}
