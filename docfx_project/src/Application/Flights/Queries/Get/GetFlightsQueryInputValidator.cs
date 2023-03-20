using System.Linq;
using Application.Common.Validators;
using Domain.Constants;
using FluentValidation;

namespace Application.Flights.Queries.Get;

public class GetFlightsQueryInputValidator : InputValidator<GetFlightsQuery>
{
    public GetFlightsQueryInputValidator()
    {
        RuleFor(x => x.Filters)
            .NotEmpty()
            .Must(filters => filters.Keys.Any(propName => propName is "origin" or "destination"))
            .WithMessage(ValidationMessages.MissingRequiredFilters);
    }
}
