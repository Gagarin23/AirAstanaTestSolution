using Application.Common.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Auth.Commands.CreateJwt;

public class CreateJwtCommandBusinessValidator : BusinessValidator<CreateJwtCommand>
{
    public CreateJwtCommandBusinessValidator(IHttpContextAccessor httpContextAccessor)
    {
        RuleFor(_ => _)
            .Must(_ => httpContextAccessor.HttpContext?.User.Identity != null);
    }
}
