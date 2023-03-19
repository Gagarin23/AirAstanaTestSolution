using System.Collections.Generic;
using System.Security.Claims;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Auth.Commands.CreateJwt;

public class CreateJwtCommand : ICommand<string>
{
    public IEnumerable<Claim> Claims { get; init; }

}
