using discipline.application.Features.Users.Configuration;
using discipline.application.Infrastructure.DAL.Connection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.Users;

internal static class BrowseMe
{
    internal static WebApplication MapBrowseMe(this WebApplication app)
    {
        // app.MapGet($"{Extensions.UsersTag}/me", 
        //         async (IDisciplineMongoCollection disciplineMongoCollection) =>
        //         {
        //             
        //         })
        //     .Produces(StatusCodes.Status200OK, typeof())
        return app;
    }
}