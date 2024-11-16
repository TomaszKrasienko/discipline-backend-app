using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using Microsoft.AspNetCore.Http;

namespace discipline.centre.shared.infrastructure.IdentityContext.Factories;

internal sealed class IdentityContextFactory(
    IHttpContextAccessor httpContextAccessor) : IIdentityContextFactory
{
    public IIdentityContext Create()
        => new Internals.IdentityContext(httpContextAccessor);
}