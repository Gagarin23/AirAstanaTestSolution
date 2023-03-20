using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Queries;

public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, AuthenticateResult>
{
    private readonly UserManager<IdentityUser> _userManager;

    public AuthenticateQueryHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<AuthenticateResult> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Login);

        if (user == null)
        {
            throw new AuthenticationException(AuthenticationMessages.PasswordOrUserIsInvalid);
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new AuthenticationException(AuthenticationMessages.PasswordOrUserIsInvalid);
        }

        var roles = await _userManager.GetRolesAsync(user);
        
        var authClaims = new List<Claim>(roles.Select(role => new Claim(ClaimTypes.Role, role)))
        {
            new Claim(ClaimTypes.Name, user.UserName),
        };

        var claims = new ClaimsPrincipal
        (
            new ClaimsIdentity(authClaims, JwtBearerDefaults.AuthenticationScheme)
        );

        var ticket = new AuthenticationTicket(claims, JwtBearerDefaults.AuthenticationScheme);
        return AuthenticateResult.Success(ticket);
    }
}
