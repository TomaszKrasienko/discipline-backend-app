using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Features.Users.Configuration;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace discipline.application.Features.Users;

internal static class BrowseMe
{
    internal static WebApplication MapBrowseMe(this WebApplication app)
    {
        app.MapGet($"{Extensions.UsersTag}/me", async (IDisciplineMongoCollection disciplineMongoCollection,
                IIdentityContext identityContext) =>
            {
                var userId = identityContext.UserId;
                var result = (await disciplineMongoCollection
                    .GetCollection<UserDocument>()
                    .Find(x => x.Id == userId.ToString())
                    .FirstOrDefaultAsync())?.AsDto();
                return result is null ? Results.NoContent() : Results.Ok(result);
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