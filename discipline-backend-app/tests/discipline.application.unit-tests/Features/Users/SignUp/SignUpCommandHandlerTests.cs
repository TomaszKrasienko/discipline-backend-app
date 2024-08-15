using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.Users;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Repositories;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.Users.SignUp;

public sealed class SignUpCommandHandlerTests
{
    private Task Act(SignUpCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingEmail_ShouldThrowEmailAlreadyExistsException()
    {
        //arrange
        var command = new SignUpCommand(Guid.NewGuid(), "test@test.pl", "Test123!",
            "first_name", "last_name");

        _userRepository
            .IsEmailExists(command.Email)
            .Returns(true);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<EmailAlreadyExistsException>();
    }

    [Fact]
    public async Task HandleAsync_GivenValidUser_ShouldAddUserWithSecuredPassword()
    {
        //arrange
        var securedPassword = Guid.NewGuid().ToString();
        var command = new SignUpCommand(Guid.NewGuid(), "test@test.pl", "Test123!",
            "test_first_name", "test_last_name");

        _userRepository
            .IsEmailExists(command.Email)
            .Returns(false);

        _passwordManager
            .Secure(command.Password)
            .Returns(securedPassword);
        
        //act
        await Act(command);
        
        //assert
        await _userRepository
            .Received(1)
            .AddAsync(Arg.Is<User>(arg
                => arg.Id == command.Id
                   && arg.Email == command.Email
                   && arg.Password == securedPassword
                   && arg.FullName.FirstName == command.FirstName
                   && arg.FullName.LastName == command.LastName));

    }
    
    #region arrange
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly ICommandHandler<SignUpCommand> _handler;

    public SignUpCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _handler = new SignUpCommandHandler(_userRepository, _passwordManager);
    }
    #endregion
}