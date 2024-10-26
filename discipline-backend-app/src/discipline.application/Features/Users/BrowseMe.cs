using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Features.Users.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.Users;

internal static class BrowseMe
{
    internal static WebApplication MapBrowseMe(this WebApplication app)
    {
        app.MapGet($"{Extensions.UsersTag}/me", (
                IIdentityContext identityContext) =>
            {
                var userId = identityContext.UserId;
                //TODO: To finish
                // var result = (await disciplineMongoCollection
                //     .GetCollection<UserDocument>()
                //     .Find(x => x.Id == userId.ToString())
                //     .FirstOrDefaultAsync())?.AsDto();
                // return result is null ? Results.NoContent() : Results.Ok(result);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status200OK, typeof(UserDto))
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .WithName(nameof(BrowseMe))
            .WithTags(Extensions.UsersTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets logged user"
            })
            .RequireAuthorization();
        return app;
    }
}