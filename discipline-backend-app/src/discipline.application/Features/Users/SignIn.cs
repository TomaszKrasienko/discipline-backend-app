using discipline.application.Behaviours;
using discipline.application.Domain.Users.Repositories;
using discipline.application.Exceptions;
using discipline.application.Features.Users.Configuration;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.Users;

internal static class SignIn
{
    internal static WebApplication MapSignIn(this WebApplication app)
    {
        app.MapPost($"{Extensions.UsersTag}/sign-in", async (SignInCommand command,
                ICommandDispatcher commandDispatcher, ITokenStorage tokenStorage, CancellationToken cancellationToken) =>
            {
                await commandDispatcher.HandleAsync(command, cancellationToken);
                var jwt = tokenStorage.Get(); 
                return Results.Ok(jwt);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
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
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    IAuthenticator authenticator,
    ITokenStorage tokenStorage) : ICommandHandler<SignInCommand>
{
    public async Task HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);
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
        tokenStorage.Set(token);
    }
}