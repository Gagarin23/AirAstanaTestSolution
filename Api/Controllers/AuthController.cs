using System.Threading;
using System.Threading.Tasks;
using Application.Auth.Commands.CreateJwt;
using Application.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер аутентификации
/// </summary>
public class AuthController : ApiController
{
    public AuthController(IMediator mediator)
        : base(mediator) { }
    
    /// <summary>
    /// Аутентификация и получение JWT
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Authenticate(AuthenticateQuery request, CancellationToken cancellationToken)
    {
        var authResult = await Mediator.Send(request, cancellationToken);

        return authResult.Succeeded ?
            Ok(await Mediator.Send(new CreateJwtCommand() { Claims = authResult.Principal.Claims }, cancellationToken)) :
            throw authResult.Failure;
    }
}
