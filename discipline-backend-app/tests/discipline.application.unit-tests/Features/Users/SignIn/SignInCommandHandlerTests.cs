using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Exceptions;
using discipline.application.Features.Users;
using discipline.domain.Users.Repositories;
using discipline.tests.shared.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.Users.SignIn;

public sealed class SignInCommandHandlerTests
{
    private Task Act(SignInCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingUserWithValidPassword_ShouldSaveByTokenStorage()
    {
        //arrange
        var user = UserFactory.Get();
        var command = new SignInCommand(user.Email, "Test123!");
        _userRepository
            .GetByEmailAsync(command.Email)
            .Returns(user);
        
        _passwordManager
            .VerifyPassword(user.Password, command.Password)
            .Returns(true);

        var token = Guid.NewGuid().ToString();
        _authenticator
            .CreateToken(user.Id.ToString(), user.Status.Value)
            .Returns(token);

        var refreshToken = Guid.NewGuid().ToString();
        _refreshTokenFacade
            .GenerateAsync(user.Id, default)
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
    public async Task HandleAsync_GivenNotExistingEmail_ShouldThrowUserNotFoundException()
    {
        //arrange
        var command = new SignInCommand("test@test.pl", "Test123!");
        
        //act
        var exception = await Record.ExceptionAsync( async() => await Act(command));
        
        //assert
        exception.ShouldBeOfType<UserNotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_GivenInvalidPassword_ShouldThrowInvalidPasswordException()
    {
        //arrange
        var user = UserFactory.Get();
        var command = new SignInCommand(user.Email, "Test123!");
        _userRepository
            .GetByEmailAsync(command.Email)
            .Returns(user);
        
        _passwordManager
            .VerifyPassword(user.Password, command.Password)
            .Returns(false);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<InvalidPasswordException>();
    }
    
    #region arrange
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;
    private readonly IRefreshTokenFacade _refreshTokenFacade;
    private readonly ICommandHandler<SignInCommand> _handler;
    
    public SignInCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _authenticator = Substitute.For<IAuthenticator>();
        _tokenStorage = Substitute.For<ITokenStorage>();
        _refreshTokenFacade = Substitute.For<IRefreshTokenFacade>();
        _handler = new SignInCommandHandler(
            _userRepository,
            _passwordManager,
            _authenticator,
            _tokenStorage,
            _refreshTokenFacade);
    }
    
    #endregion
}