using discipline.application.Behaviours;
using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Repositories;
using discipline.application.DTOs;
using discipline.application.Exceptions;
using discipline.application.Features.Users;
using discipline.tests.shared.Entities;
using Microsoft.AspNetCore.Routing.Matching;
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
        var subscription = SubscriptionFactory.Get();
        var command = new SignInCommand(user.Email, "Test123!");
        _userRepository
            .GetByEmailAsync(command.Email)
            .Returns(user);

        _subscriptionRepository
            .GetByIdAsync(user.SubscriptionOrder.SubscriptionId)
            .Returns(subscription);
        
        _passwordManager
            .VerifyPassword(user.Password, command.Password)
            .Returns(true);

        var jwtDto = new JwtDto()
        {
            Token = Guid.NewGuid().ToString()
        };
        _authenticator
            .CreateToken(user.Id.Value.ToString(), subscription.Title, null)
            .Returns(jwtDto);
        
        //act
        await Act(command);
        
        //assert
        _tokenStorage
            .Received(1)
            .Set(jwtDto);
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
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;
    private readonly ICommandHandler<SignInCommand> _handler;
    
    public SignInCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _authenticator = Substitute.For<IAuthenticator>();
        _handler = new SignInCommandHandler();
    }
    
    #endregion
}