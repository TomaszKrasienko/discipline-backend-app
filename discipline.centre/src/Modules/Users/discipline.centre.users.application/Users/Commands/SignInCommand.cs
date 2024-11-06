using discipline.centre.shared.abstractions.CQRS.Commands;
using FluentValidation;

namespace discipline.centre.users.application.Users.Commands;

public sealed record SignInCommand(string Email, string Password) : ICommand;

public sealed class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email can not be null or empty")
            .EmailAddress()
            .WithMessage("Email is invalid");
        
        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password can not be null or empty");
    }
}