using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.Users.Configuration;

internal static class Extensions
{
    internal const string UsersTag = "users";
    
    internal static WebApplication MapUserFeatures(this WebApplication app)
        => app
            .MapCreateUserSubscriptionOrder()
            .MapSignUp()
            .MapSignIn()
            .MapBrowseSubscriptions();
}