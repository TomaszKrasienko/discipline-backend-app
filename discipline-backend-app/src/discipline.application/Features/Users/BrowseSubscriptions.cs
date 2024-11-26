using discipline.application.DTOs;
using discipline.application.Features.Users.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.Users;

internal static class BrowseSubscriptions
{
    internal static WebApplication MapBrowseSubscriptions(this WebApplication app)
    {
        app.MapGet($"{Extensions.UsersTag}/subscriptions", () =>
                {
                    // var results = await disciplineMongoCollection.GetCollection<SubscriptionDocument>()
                    //     .Find(_ => true).ToListAsync();
                    // return Results.Ok(results);
                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status200OK, typeof(List<SubscriptionDto>))
            .WithName(nameof(BrowseSubscriptions))
            .WithTags(Extensions.UsersTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Browses subscriptions"
            });
        return app;
    }
}