using discipline.domain.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours;

internal static class IdentityFromContextBehaviour
{
    internal static IServiceCollection AddIdentityFromContextBehaviour(this IServiceCollection services)
        => services
            .AddHttpContextAccessor()
            .AddSingleton<IIdentityContextFactory, IdentityContextFactory>()
            .AddScoped<IIdentityContext>(sp =>
            {
                var factory = sp.GetRequiredService<IIdentityContextFactory>();
                return factory.Create();
            });
}

public interface IIdentityContext
{
    public bool IsAuthenticated { get; }
    public UserId UserId { get; }
    public string Status { get; }
}

internal sealed class IdentityContext : IIdentityContext
{
    public bool IsAuthenticated { get; }
    public UserId UserId { get; }
    public string Status { get; }

    public IdentityContext(HttpContext httpContext)
    {
        var user = httpContext.User;
        IsAuthenticated = user.Identity?.IsAuthenticated ?? false;

        if (!IsAuthenticated)
        {
            return;
        }

        if (!Ulid.TryParse(user.Identity?.Name, out var userId))
        {
            throw new ArgumentException("Bad user id");
        }

        UserId = new UserId(userId);
        Status = user.Claims.SingleOrDefault(x => x.Type == "Status")?.Value;
    }
}

public interface IIdentityContextFactory
{
    IIdentityContext Create();
}

internal sealed class IdentityContextFactory(
    IHttpContextAccessor httpContextAccessor) : IIdentityContextFactory
{
    public IIdentityContext Create()
        => new IdentityContext(httpContextAccessor.HttpContext);
}