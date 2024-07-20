using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.Users.Configuration;

internal static class Extensions
{
    internal static WebApplication MapUserFeatures(this WebApplication app)
        => app
            .MapSignUp();
}