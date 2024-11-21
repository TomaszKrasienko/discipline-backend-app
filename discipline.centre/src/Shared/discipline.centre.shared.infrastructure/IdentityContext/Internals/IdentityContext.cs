using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using Microsoft.AspNetCore.Http;

namespace discipline.centre.shared.infrastructure.IdentityContext.Internals;

internal sealed class IdentityContext : IIdentityContext
{
    public bool IsAuthenticated { get; }
    public UserId? UserId { get; }
    public string? Status { get; }

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

        UserId = new UserId(userId);
        Status = user.Claims.SingleOrDefault(x => x.Type == "Status")?.Value;
    }
}