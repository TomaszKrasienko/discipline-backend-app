using discipline.application.Infrastructure.DAL.Connection;
using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.Users;

internal static class BrowseSubscriptions
{
    internal static WebApplication MapBrowseSubscriptions(this WebApplication app)
    {
        app.MapGet("subscriptions", async (IDisciplineMongoCollection disciplineMongoCollection) =>
        {
            
        });
        return app;
    }
}