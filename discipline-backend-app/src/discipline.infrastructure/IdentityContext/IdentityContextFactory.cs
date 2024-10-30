using discipline.application.Behaviours.IdentityContext;
using Microsoft.AspNetCore.Http;

namespace discipline.infrastructure.IdentityContext;

internal sealed class IdentityContextFactory(
    IHttpContextAccessor httpContextAccessor) : IIdentityContextFactory
{
    public IIdentityContext Create()
        => new IdentityContext(httpContextAccessor);
}