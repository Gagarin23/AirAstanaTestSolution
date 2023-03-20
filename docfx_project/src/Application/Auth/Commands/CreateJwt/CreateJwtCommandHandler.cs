using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Auth.Commands.CreateJwt;

public class CreateJwtCommandHandler : IRequestHandler<CreateJwtCommand, string>
{
    private readonly JwtBearerOptions _jwtOptions;

    public CreateJwtCommandHandler(IOptionsMonitor<JwtBearerOptions> monitor)
    {
        _jwtOptions = monitor.Get(JwtBearerDefaults.AuthenticationScheme);
    }
    
    public Task<string> Handle(CreateJwtCommand request, CancellationToken cancellationToken)
    {
        var securityKey = _jwtOptions.TokenValidationParameters.IssuerSigningKey;
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.TokenValidationParameters.ValidIssuer,
            Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
            SigningCredentials = credentials,
            Claims = request.Claims.ToDictionary
            (
                keySelector: claim => claim.Type,
                elementSelector: claim => (object)string.Join(',', claim.Value)
            )
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}
