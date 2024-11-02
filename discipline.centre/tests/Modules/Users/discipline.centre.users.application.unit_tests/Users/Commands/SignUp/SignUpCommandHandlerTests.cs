using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Events;
using discipline.centre.users.domain.Users.Repositories;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.application.unit_tests.Users.Commands.SignUp;

public sealed class SignUpCommandHandlerTests
{
    private Task Act(SignUpCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenNotRegisteredEmail_ShouldAddUserAndProcessEvent()
    {
        //arrange
        var command = new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            "test_first_name", "test_last_name");

        _readUserRepository
            .DoesEmailExist(command.Email)
            .Returns(false);
        
        //act
        await Act(command);
        
        //assert
        await _writeUserRepository
            .Received(1)
            .AddAsync(Arg.Is<User>(arg 
                => arg.Id == command.Id 
                && arg.Email == command.Email 
                && arg.Password.Value == command.Password 
                && arg.FullName.FirstName == command.FirstName 
                && arg.FullName.LastName == command.LastName));
        
        await _eventProcessor
            .Received(1)
            .PublishAsync(Arg.Is<UserCreated>(arg 
                => arg.UserId == command.Id
                && arg.Email.Value == command.Email));
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingEmail_ShouldThrow()
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
        exception.ShouldBeOfType<AlreadyRegisteredException>();
        ((AlreadyRegisteredException)exception).Code.ShouldBe("SignUpCommand.Email");
    }


    
    #region arrange

    private readonly IReadUserRepository _readUserRepository;
    private readonly IWriteUserRepository _writeUserRepository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICommandHandler<SignUpCommand> _handler;

    public SignUpCommandHandlerTests()
    {
        _readUserRepository = Substitute.For<IReadUserRepository>();
        _writeUserRepository = Substitute.For<IWriteUserRepository>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _handler = new SignUpCommandHandler(_readUserRepository,
            _writeUserRepository, _eventProcessor);
    }
    #endregion
}