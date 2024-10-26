using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.Users;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Events;
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
        var command = new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            "first_name", "last_name");

        _readUserRepository
            .DoesEmailExist(command.Email, default)
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
        var command = new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            "test_first_name", "test_last_name");

        _readUserRepository
            .DoesEmailExist(command.Email)
            .Returns(false);

        _passwordManager
            .Secure(command.Password)
            .Returns(securedPassword);
        
        //act
        await Act(command);
        
        //assert
        await _writeUserRepository
            .Received(1)
            .AddAsync(Arg.Is<User>(arg
                => arg.Id == command.Id
                   && arg.Email == command.Email
                   && arg.Password == securedPassword
                   && arg.FullName.FirstName == command.FirstName
                   && arg.FullName.LastName == command.LastName));
        
        await _eventProcessor
            .Received(1)
            .PublishAsync(Arg.Is<UserCreated>(arg 
                => arg.UserId == command.Id
                && arg.Email.Value == command.Email));
    }
    
    #region arrange

    private readonly IReadUserRepository _readUserRepository;
    private readonly IWriteUserRepository _writeUserRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICommandHandler<SignUpCommand> _handler;

    public SignUpCommandHandlerTests()
    {
        _readUserRepository = Substitute.For<IReadUserRepository>();
        _writeUserRepository = Substitute.For<IWriteUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _handler = new SignUpCommandHandler(_readUserRepository,
            _writeUserRepository, _passwordManager, _eventProcessor);
    }
    #endregion
}