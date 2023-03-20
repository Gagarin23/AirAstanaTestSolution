using Application.Common.Validators;
using FluentValidation;

namespace Application.Flights.Commands.UpdateStatus;

public class UpdateFlightStatusCommandInputValidator : InputValidator<UpdateFlightStatusCommand>
{
    public UpdateFlightStatusCommandInputValidator()
    {
        RuleFor(x => x.FlightId)
            .NotEmpty();

        //Не Undefined
        RuleFor(x => x.Status)
            .NotEmpty();
    }
}
