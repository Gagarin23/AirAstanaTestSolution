using System.Collections.Generic;

namespace Application.Flights.Queries.Get;

public class GetFlightsQueryResponse
{
    public List<GetFlightsQueryFlightDto> Flights { get; }
    
    public GetFlightsQueryResponse(List<GetFlightsQueryFlightDto> flights)
    {
        Flights = flights;
    }
}
