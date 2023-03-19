using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace Application.Auth.Queries;

public class AuthenticateQuery : IRequest<AuthenticateResult>
{
    public string Login { get; set; }
    public string Password { get; set; }
}
