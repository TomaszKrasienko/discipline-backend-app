using discipline.application.Behaviours;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Behaviours.Events;
using discipline.application.Exceptions;
using discipline.application.Features.Users.Configuration;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users;
using discipline.domain.Users.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace discipline.application.Features.Users;

internal static class SignUp
{
    internal static WebApplication MapSignUp(this WebApplication app)
    {
        app.MapPost($"{Extensions.UsersTag}/sign-up", async (SignUpCommand command,
            ICqrsDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var userId = UserId.New();
                await commandDispatcher.HandleAsync(command with {Id = userId}, cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
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
            .Must(id => id != new UserId(Ulid.Empty))
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
    IReadUserRepository readUserRepository,
    IWriteUserRepository writeUserRepository,
    IPasswordManager passwordManager,
    IEventProcessor eventProcessor) : ICommandHandler<SignUpCommand>
{
    public async Task HandleAsync(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var doesEmailExist = await readUserRepository.DoesEmailExist(command.Email, cancellationToken);
        if (!doesEmailExist)
        {
            throw new EmailAlreadyExistsException(command.Email);
        }

        var securedPassword = passwordManager.Secure(command.Password);
        var user = User.Create(command.Id, command.Email, securedPassword, command.FirstName, command.LastName);
        await writeUserRepository.AddAsync(user, cancellationToken);
        await eventProcessor.PublishAsync(user.DomainEvents.ToArray());
    }
}