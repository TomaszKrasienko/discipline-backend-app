using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.Users.Configuration;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.Users;

internal static class SignUp
{
    internal static WebApplication MapSignUp(this WebApplication app)
    {
        app.MapPost($"{Extensions.UsersTag}/sign-up", async (SignUpCommand command,
            ICommandDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var userId = UserId.New();
                await commandDispatcher.HandleAsync(command with {Id = userId}, cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(SignUp))
            .WithTags(Extensions.UsersTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Signs-up user"
            });
        return app;
    }
}

public sealed record SignUpCommand(UserId Id, string Email, string Password, string FirstName, string LastName) : ICommand;

public sealed class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User \"ID\" can not be empty");

        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("User \"Email\" can not be empty");
            
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("User \"Email\" is invalid");

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .Must(x => x.Any(char.IsLower))
            .Must(x => x.Any(char.IsUpper))
            .Must(x => x.Any(char.IsNumber))
            .Must(x => x.Any(c => !char.IsLetterOrDigit(c)))
            .WithMessage("User \"Password\" is invalid");

        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .WithMessage("User \"First name\" can not be empty");
        
        RuleFor(x => x.FirstName)
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("User \"First name\" is invalid");

        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .WithMessage("User \"Last name\" can not be empty");
        
        RuleFor(x => x.LastName)
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("User \"Last name\" is invalid");
    }
}

internal sealed class SignUpCommandHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    IEventPublisher eventPublisher) : ICommandHandler<SignUpCommand>
{
    public async Task HandleAsync(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        if (await userRepository.IsEmailExists(command.Email, cancellationToken))
        {
            throw new EmailAlreadyExistsException(command.Email);
        }

        var securedPassword = passwordManager.Secure(command.Password);
        var user = User.Create(command.Id, command.Email, securedPassword, command.FirstName, command.LastName);
        await userRepository.AddAsync(user, cancellationToken);
        await eventPublisher.PublishAsync(new UserSignedUp(command.Id.Value));
    }
}

internal sealed record UserSignedUp(Ulid UserId) : IEvent;