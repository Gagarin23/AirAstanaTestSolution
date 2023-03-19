using System;
using Application.Common.Extensions;
using Domain.Entities;
using Domain.Entities.FlightAggregate;
using HarmonyLib;
using Infrastructure.DbEntities;
using Mapster;

namespace Infrastructure.Mappings;

public class FlightDbModelMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<FlightDbModel, Flight>()
            .Ignore(flight => flight.Status)
            //FlightStatus.InTime - хак для маппинга.
            .ConstructUsing(model => new Flight(model.Origin, model.Destination, model.Departure, model.Arrival, FlightStatus.InTime))
            .AfterMapping(
                (model, flight) =>
                {
                    var statusInfo = AccessTools.Field(typeof(Flight), "<Status>k__BackingField");
                    ref var statusField = ref AccessTools.FieldRefAccess<Flight, FlightStatus>(statusInfo)(flight);
                    statusField = model.StatusId.ToFlightStatus();
                        
                    var idInfo = AccessTools.Field(typeof(AggregateBase), "<Id>k__BackingField");
                    ref var idField = ref AccessTools.FieldRefAccess<AggregateBase, Guid>(idInfo)(flight);
                    idField = model.Id;
                });
        
        config.ForType<Flight, FlightDbModel>()
            .Ignore(x => x.Status)
            .ConstructUsing(flight => new FlightDbModel())
            .AfterMapping(
                (flight, model) =>
                {
                    var statusInfo = AccessTools.Field(typeof(FlightDbModel), "<StatusId>k__BackingField");
                    ref var statusField = ref AccessTools.FieldRefAccess<FlightDbModel, Guid>(statusInfo)(model);
                    statusField = flight.Status.ToGuid();
                });
    }
}
