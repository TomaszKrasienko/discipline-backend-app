using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Exceptions;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Users.Repositories;
using discipline.centre.users.tests.sharedkernel.Domain;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.users.application.unit_tests.Users.Commands.SignIn;

public sealed class SignInCommandHandlerTests
{
    private Task Act(SignInCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingUserWithValidPassword_ShouldSaveByTokenStorage()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var command = new SignInCommand(user.Email, "Test123!");
        
        _readUserRepository
            .GetByEmailAsync(command.Email, default)
            .Returns(user);
        
        _passwordManager
            .VerifyPassword(user.Password.HashedValue!, command.Password)
            .Returns(true);

        var token = Guid.NewGuid().ToString();
        
        _authenticator
            .CreateToken(user.Id.ToString(), user.Email, user.Status)
            .Returns(token);

        var refreshToken = Guid.NewGuid().ToString();
        
        _refreshTokenFacade
            .GenerateAndSaveAsync(user.Id, default)
            .Returns(refreshToken);
        
        //act
        await Act(command);
        
        //assert
        _tokenStorage
            .Received(1)
            .Set(Arg.Is<TokensDto>(arg
                => arg.Token == token
                && arg.RefreshToken == refreshToken));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingEmail_ShouldThrowNotFoundException()
    {
        //arrange
        var command = new SignInCommand("test@test.pl", "Test123!");
        
        //act
        var exception = await Record.ExceptionAsync( async() => await Act(command));
        
        //assert
        exception.ShouldBeOfType<NotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_GivenInvalidPassword_ShouldThrowInvalidPasswordException()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var command = new SignInCommand(user.Email, "Test123!");
        _readUserRepository
            .GetByEmailAsync(command.Email, default)
            .Returns(user);
        
        _passwordManager
            .VerifyPassword(user.Password.HashedValue!, command.Password)
            .Returns(false);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<InvalidPasswordException>();
    }
    
    #region arrange

    private readonly IReadUserRepository _readUserRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;
    private readonly IRefreshTokenFacade _refreshTokenFacade;
    private readonly ICommandHandler<SignInCommand> _handler;
    
    public SignInCommandHandlerTests()
    {
        _readUserRepository = Substitute.For<IReadUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _authenticator = Substitute.For<IAuthenticator>();
        _tokenStorage = Substitute.For<ITokenStorage>();
        _refreshTokenFacade = Substitute.For<IRefreshTokenFacade>();
        _handler = new SignInCommandHandler(
            _readUserRepository,
            _passwordManager,
            _authenticator,
            _tokenStorage,
            _refreshTokenFacade);
    }
    
    #endregion
}