using System.Linq;
using System.Security.Claims;
using Application.Common.Interfaces;
using Infrastructure.Interfaces;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Users;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string _username;

    [CanBeNull]
    public string Username =>
        _username ??= _httpContextAccessor.HttpContext?
            .User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
