using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Exceptions;
using discipline.application.Features.Users;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Repositories;
using discipline.tests.shared.Entities;
using Microsoft.AspNetCore.Builder;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.Users.RefreshUserToken;

public sealed class RefreshTokenCommandHandlerTests
{
    private Task Act(RefreshTokenCommand command) => _handler.HandleAsync(command);

    [Fact]
    public async Task HandleAsync_GivenValidRefreshToken_ShouldSaveByTokenStorage()
    {
        //arrange
        var command = new RefreshTokenCommand(Guid.NewGuid().ToString());
        var user = UserFactory.Get();

        _refreshTokenFacade
            .GetUserIdAsync(command.RefreshToken)
            .Returns(user.Id);

        _userRepository
            .GetByIdAsync(user.Id)
            .Returns(user);

        var token = Guid.NewGuid().ToString();
        _authenticator
            .CreateToken(user.Id.ToString(), user.Status)
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
    public async Task HandleAsync_GivenRefreshTokenForNotExistingUser_ShouldThrowUserNotFoundException()
    {
        //arrange
        var command = new RefreshTokenCommand(Guid.NewGuid().ToString());

        _refreshTokenFacade
            .GetUserIdAsync(command.RefreshToken)
            .Returns(Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<UserNotFoundException>();
    }
    
    #region arrange
    private readonly IRefreshTokenFacade _refreshTokenFacade;
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;
    private readonly ICommandHandler<RefreshTokenCommand> _handler;
    
    public RefreshTokenCommandHandlerTests()
    {
        _refreshTokenFacade = Substitute.For<IRefreshTokenFacade>();
        _userRepository = Substitute.For<IUserRepository>();
        _authenticator = Substitute.For<IAuthenticator>();
        _tokenStorage = Substitute.For<ITokenStorage>();
        _handler = new RefreshTokenCommandHandler(
            _refreshTokenFacade,
            _userRepository,
            _authenticator,
            _tokenStorage);
    }
    
    #endregion
}