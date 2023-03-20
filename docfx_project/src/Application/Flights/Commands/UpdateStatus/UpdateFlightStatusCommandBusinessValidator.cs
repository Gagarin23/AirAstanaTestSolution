using Application.Common.Validators;
using Domain.Constants;
using Domain.Entities.FlightAggregate;
using FluentValidation;

namespace Application.Flights.Commands.UpdateStatus;

public class UpdateFlightStatusCommandValidator : BusinessValidator<UpdateFlightStatusCommand>
{
    public UpdateFlightStatusCommandValidator()
    {
        When
        (
            x => x.Status == FlightStatus.Delayed,
            () => RuleFor(x => x)
                .Must(x => x.DepartureOffsetInMinutes != default || x.ArrivalOffsetInMinutes != default)
                .WithMessage(ValidationMessages.DepartureOrArrivalWhenDelay)
        );
    }
}
