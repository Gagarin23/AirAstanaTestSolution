using System.Collections.Generic;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Flights.Queries.Get;

public class GetFlightsQuery : IQuery<GetFlightsQueryResponse>
{
    //Используем интерфейс, т.к. даем платформе самой выбирать тип для сериализации.
    //Возможно, с развитием платформы появится более производительный тип для сериализациии.
    public IDictionary<string, string> Filters { get; set; }
}
