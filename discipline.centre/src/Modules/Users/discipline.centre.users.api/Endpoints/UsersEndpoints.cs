using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace discipline.centre.users.api.Endpoints;

internal static class UsersEndpoints
{
    private const string UserTag = "users";
    
    internal static WebApplication MapUsersEndpoints(this WebApplication app)
    {
        app.MapPost($"{UsersModule.ModuleName}/{UserTag}/sign-up", async (SignUpCommand command,
                ICqrsDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var userId = UserId.New();
                await commandDispatcher.HandleAsync(command with {Id = userId}, cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("SignUp")
            .WithTags(UserTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Signs-up user"
            });
        return app;
    }
}