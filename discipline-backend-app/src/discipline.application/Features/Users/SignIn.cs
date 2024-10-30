using discipline.application.Behaviours;
using discipline.application.Behaviours.Auth;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Behaviours.Passwords;
using discipline.application.Behaviours.RefreshToken;
using discipline.application.DTOs;
using discipline.application.Exceptions;
using discipline.application.Features.Users.Configuration;
using discipline.domain.Users.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace discipline.application.Features.Users;

internal static class SignIn
{
    internal static WebApplication MapSignIn(this WebApplication app)
    {
        app.MapPost($"{Extensions.UsersTag}/sign-in", async (SignInCommand command,
                ICqrsDispatcher commandDispatcher, ITokenStorage tokenStorage, CancellationToken cancellationToken) =>
            {
                await commandDispatcher.HandleAsync(command, cancellationToken);
                var jwt = tokenStorage.Get(); 
                return Results.Ok(jwt);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName(nameof(SignIn))
            .WithTags(Extensions.UsersTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Signs-in user"
            });
        return app;
    }
}

public sealed record SignInCommand(string Email, string Password) : ICommand;

public sealed class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email can not be null or empty");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email is invalid");
        
        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password can not be null or empty");
    }
}

internal sealed class SignInCommandHandler(
    IReadUserRepository readUserRepository,
    IWriteUserRepository writeUserRepository,
    IPasswordManager passwordManager,
    IAuthenticator authenticator,
    ITokenStorage tokenStorage,
    IRefreshTokenFacade refreshTokenFacade) : ICommandHandler<SignInCommand>
{
    public async Task HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var user = await readUserRepository.GetAsync(x => x.Email == command.Email, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(command.Email);
        }

        var isPasswordValid = passwordManager.VerifyPassword(user.Password, command.Password);
        if (!isPasswordValid)
        {
            throw new InvalidPasswordException();
        }

        var token = authenticator.CreateToken(user.Id.ToString(), user.Status);
        var refreshToken = await refreshTokenFacade.GenerateAsync(user.Id, cancellationToken);
        tokenStorage.Set(new TokensDto()
        {
            Token = token,
            RefreshToken = refreshToken
        });
    }
}