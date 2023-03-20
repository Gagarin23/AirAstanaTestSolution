using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace Application.Auth.Queries;

public class AuthenticateQuery : IQuery<AuthenticateResult>
{
    public string Login { get; set; }
    public string Password { get; set; }
}
