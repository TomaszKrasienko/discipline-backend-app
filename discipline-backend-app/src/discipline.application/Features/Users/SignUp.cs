using discipline.application.Behaviours;
using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Repositories;
using discipline.application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.Users;

internal static class SignUp
{
    internal static WebApplication MapSignUp(this WebApplication app)
    {
        //app.MapPost()
        return app;
    }
}

public sealed record SignUpCommand(Guid Id, string Email, string Password, string FirstName, string LastName) : ICommand;

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
    IPasswordManager passwordManager) : ICommandHandler<SignUpCommand>
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
    }
}