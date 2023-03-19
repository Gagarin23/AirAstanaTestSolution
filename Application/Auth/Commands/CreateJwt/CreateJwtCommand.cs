﻿using System.Collections.Generic;
using System.Security.Claims;
using MediatR;

namespace Application.Auth.Commands.CreateJwt;

public class CreateJwtCommand : IRequest<string>
{
    public IEnumerable<Claim> Claims { get; init; }

}
