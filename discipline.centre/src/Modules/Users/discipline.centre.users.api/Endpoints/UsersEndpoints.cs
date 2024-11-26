using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using discipline.centre.shared.infrastructure.ResourceHeader;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.application.Users.Queries;
using discipline.centre.users.application.Users.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable All
namespace discipline.centre.users.api.Endpoints;

internal static class UsersEndpoints
{
    private const string UserTag = "users";
    private const string GetById = "GetById";
    
    internal static WebApplication MapUsersEndpoints(this WebApplication app)
    {
        app.MapPost($"{UsersModule.ModuleName}/{UserTag}", async (SignUpCommand command,
            CancellationToken cancellationToken, ICqrsDispatcher commandDispatcher, IHttpContextAccessor contextAccessor) =>
            {
                var userId = UserId.New();
                await commandDispatcher.HandleAsync(command with {Id = userId}, cancellationToken);
                contextAccessor.AddResourceIdHeader(userId.ToString());
                
                return Results.CreatedAtRoute(nameof(GetById),  new {userId = userId.ToString()}, null);
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
        
        app.MapPost($"{UsersModule.ModuleName}/{UserTag}/tokens", async (SignInCommand command,
                ICqrsDispatcher commandDispatcher, ITokenStorage tokenStorage, CancellationToken cancellationToken) =>
            {
                await commandDispatcher.HandleAsync(command, cancellationToken);
                var jwt = tokenStorage.Get(); 
                
                return Results.Ok(jwt);
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("SignIn")
            .WithTags(UserTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Signs-in user"
            });
        
        app.MapPost($"{UsersModule.ModuleName}/{UserTag}/subscription-order", async (CreateUserSubscriptionOrderCommand command,
                IIdentityContext identityContext, ICqrsDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var subscriptionOrderId = SubscriptionOrderId.New();
                await commandDispatcher.HandleAsync(command with { Id = subscriptionOrderId, UserId = identityContext.GetUser() },
                    cancellationToken);
                
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("CreateUserSubscriptionOrder")
            .WithTags(UserTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds subscription order for user"
            })
            .RequireAuthorization();
        
        app.MapGet($"{UsersModule.ModuleName}/{UserTag}/{{userId:ulid}}", async (Ulid userId,
                CancellationToken cancellationToken, ICqrsDispatcher dispatcher) =>
            {
                var stronglyUserId = new UserId(userId);
                var result = await dispatcher.SendAsync(new GetUserByIdQuery(stronglyUserId), cancellationToken);
                
                return result is null ? Results.NotFound() : Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
            .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
            .WithName(GetById)
            .WithTags(UserTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Gets user by identifier"
            })
            .RequireAuthorization();
            
        return app;
    }
}