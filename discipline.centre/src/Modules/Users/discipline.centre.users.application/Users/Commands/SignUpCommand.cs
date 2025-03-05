using System.Runtime.CompilerServices;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Events;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Repositories;
using FluentValidation;

namespace discipline.centre.users.application.Users.Commands;

public sealed record SignUpCommand(UserId Id, string Email, string Password, string FirstName, string LastName) : ICommand;

public sealed class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("User 'Email' can not be empty")
            .EmailAddress()
            .WithMessage("User 'Email' is invalid");

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(8)
            .Must(x => x.Any(char.IsLower))
            .Must(x => x.Any(char.IsUpper))
            .Must(x => x.Any(char.IsNumber))
            .Must(x => x.Any(c => !char.IsLetterOrDigit(c)))
            .WithMessage("User 'Password' is invalid");

        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .WithMessage("User \"First name\" can not be empty");
        
        RuleFor(x => x.FirstName)
            .MinimumLength(2)
            .MaximumLength(100)
            .WithMessage("User \"First name\" is invalid");

        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .WithMessage("User \"Last name\" can not be empty")
            .MinimumLength(2)
            .MaximumLength(100)
            .WithMessage("User \"Last name\" is invalid");
    }
}

internal sealed class SignUpCommandHandler(
    IReadWriteUserRepository readWriteUserRepository,
    IEventProcessor eventProcessor) : ICommandHandler<SignUpCommand>
{
    public async Task HandleAsync(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var doesEmailExist = await readWriteUserRepository.DoesEmailExistAsync(command.Email, cancellationToken);
        if (doesEmailExist)
        {
            throw new AlreadyRegisteredException("SignUpCommand.Email", command.Email);
        }
        
        var user = User.Create(command.Id, command.Email, command.Password, command.FirstName, command.LastName);
        await readWriteUserRepository.AddAsync(user, cancellationToken);
        await eventProcessor.PublishAsync(user.DomainEvents.Select(x 
            => x.MapAsIntegrationEvent()).ToArray());
    }
}