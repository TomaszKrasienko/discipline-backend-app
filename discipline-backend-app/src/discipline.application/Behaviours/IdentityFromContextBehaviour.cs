using Microsoft.AspNetCore.Http;

namespace discipline.application.Behaviours;

public class IdentityFromContextBehaviour
{
    
}

public interface IIdentityContext
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
    public string Status { get; }
}

public sealed class IdentityContext : IIdentityContext
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
    public string Status { get; }

    public IdentityContext(HttpContext httpContext)
    {
        var user = httpContext.User;
        IsAuthenticated = user.Identity?.IsAuthenticated ?? false;

        if (!IsAuthenticated)
        {
            return;
        }

        if (!Guid.TryParse(user.Identity?.Name, out var userId))
        {
            throw new ArgumentException("Bad user id");
        }

        UserId = userId;
        Status = user.Claims.SingleOrDefault(x => x.Type == "Status")?.Value;
    }
}

public interface IIdentityContextFactory
{
    
}

