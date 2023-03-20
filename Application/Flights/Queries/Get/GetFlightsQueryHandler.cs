using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Mapster;
using MediatR;

namespace Application.Flights.Queries.Get;

public class GetFlightsQueryHandler : IRequestHandler<GetFlightsQuery, GetFlightsQueryResponse>
{
    private readonly IReadonlyFlightManager _readonlyFlightManager;

    public GetFlightsQueryHandler(IReadonlyFlightManager readonlyFlightManager)
    {
        _readonlyFlightManager = readonlyFlightManager;
    }
    
    public async Task<GetFlightsQueryResponse> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
    {
        var flights = await _readonlyFlightManager.GetAsync(request.Filters, cancellationToken);

        return new GetFlightsQueryResponse(flights.Adapt<List<GetFlightsQueryFlightDto>>());
    }
}
