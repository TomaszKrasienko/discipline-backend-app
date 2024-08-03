using discipline.application.Behaviours;
using FluentValidation;

namespace discipline.application.Features.Users;

internal class SignIn
{
    
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

internal sealed class SignInCommandHandler : ICommandHandler<SignInCommand>
{
    public Task HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}