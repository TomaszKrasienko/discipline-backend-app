using discipline.application.Behaviours;
using FluentValidation;

namespace discipline.application.Features.Users;

internal static class RefreshUserToken
{
    
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

internal sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand>
{
    public Task HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}