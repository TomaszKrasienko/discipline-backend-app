using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Exceptions;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Repositories;
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

internal sealed class SignInCommandHandler(
    IReadUserRepository readUserRepository,
    IPasswordManager passwordManager,
    IAuthenticator authenticator,
    ITokenStorage tokenStorage,
    IRefreshTokenFacade refreshTokenFacade) : ICommandHandler<SignInCommand>
{
    public async Task HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var user = await readUserRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("SignInCommand.User", nameof(User), command.Email);
        }

        var isPasswordValid = passwordManager.VerifyPassword(user.Password.HashedValue!, command.Password);
        if (!isPasswordValid)
        {
            throw new InvalidPasswordException();
        }

        var token = authenticator.CreateToken(user.Id.ToString(), user.Email, user.Status);
        var refreshToken = await refreshTokenFacade.GenerateAndSaveAsync(user.Id, cancellationToken);
        tokenStorage.Set(new TokensDto(token, refreshToken));
    }
}