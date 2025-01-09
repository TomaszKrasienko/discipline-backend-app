using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using Microsoft.AspNetCore.Http;

namespace discipline.centre.shared.infrastructure.IdentityContext.Internals;

internal sealed class IdentityContext : IIdentityContext
{
    private readonly UserId? _userId;
    public bool IsAuthenticated { get; }
    public string? Status { get; }
    public UserId GetUser()
    {            
        if (_userId is null)
        {
            throw new UnauthorizedException();
        }

        return _userId;
    }

    public IdentityContext(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor?.HttpContext is null)
        {
            return;
        }
        
        var user = httpContextAccessor.HttpContext.User;
        IsAuthenticated = user.Identity?.IsAuthenticated ?? false;

        if (!IsAuthenticated)
        {
            return;
        }

        if (!Ulid.TryParse(user.Identity?.Name, out var userId))
        {
            throw new ArgumentException("Invalid userId format");
        }

        _userId = new UserId(userId);
        Status = user.Claims.SingleOrDefault(x => x.Type == "Status")?.Value;
    }
}