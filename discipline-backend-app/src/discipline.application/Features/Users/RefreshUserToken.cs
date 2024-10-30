using discipline.application.Behaviours;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.DTOs;
using discipline.application.Exceptions;
using discipline.application.Features.Users.Configuration;
using discipline.domain.Users.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace discipline.application.Features.Users;

internal static class RefreshUserToken
{
    internal static WebApplication MapRefreshToken(this WebApplication app)
    {
        app.MapPost($"{Extensions.UsersTag}/refresh-token", async (RefreshTokenCommand command,
            ICqrsDispatcher commandDispatcher, ITokenStorage tokenStorage, CancellationToken cancellationToken) =>
            {
                await commandDispatcher.HandleAsync(command, cancellationToken);
                var jwt = tokenStorage.Get();
                return Results.Ok(jwt);
            })
        .Produces(StatusCodes.Status200OK, typeof(void))
        .Produces(StatusCodes.Status401Unauthorized, typeof(void))
        .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
        .WithName(nameof(RefreshUserToken))
        .WithTags(Extensions.UsersTag)
        .WithOpenApi(operation => new (operation)
        {
            Description = "Refreshes user token by refresh token"
        });
        return app;
    }
}

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotNull()
            .NotEmpty()
            .WithMessage("Refresh token can not be empty");
    }
}

internal sealed class RefreshTokenCommandHandler(
    IRefreshTokenFacade refreshTokenFacade,
    IReadUserRepository readUserRepository,
    IWriteUserRepository writeUserRepository,
    IAuthenticator authenticator,
    ITokenStorage tokenStorage) : ICommandHandler<RefreshTokenCommand>
{
    public async Task HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default)
    {
        var userId = await refreshTokenFacade.GetUserIdAsync(command.RefreshToken, cancellationToken);
        var user = await readUserRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }

        var token = authenticator.CreateToken(user.Id.ToString(), user.Status);
        var refreshToken = await refreshTokenFacade.GenerateAsync(userId, cancellationToken);
        tokenStorage.Set(new TokensDto()
        {
            Token = token,
            RefreshToken = refreshToken
        });
    }
}