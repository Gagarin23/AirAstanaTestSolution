using Application.Common.Validators;
using FluentValidation;

namespace Application.Auth.Queries;

public class AuthenticateQueryValidator : InputValidator<AuthenticateQuery>
{
    public AuthenticateQueryValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
