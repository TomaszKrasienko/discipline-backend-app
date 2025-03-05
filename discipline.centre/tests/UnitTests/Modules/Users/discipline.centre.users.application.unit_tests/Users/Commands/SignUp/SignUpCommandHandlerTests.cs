using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.application.Users.Events;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Events;
using discipline.centre.users.domain.Users.Repositories;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.users.application.unit_tests.Users.Commands.SignUp;

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
            .DoesEmailExistAsync(command.Email)
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
            .PublishAsync(Arg.Is<UserSignedUp>(arg 
                => arg.UserId == command.Id
                && arg.Email == command.Email));
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingEmail_ShouldThrowAlreadyRegisteredException()
    {
        //arrange
        var command = new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            "first_name", "last_name");

        _readUserRepository
            .DoesEmailExistAsync(command.Email, default)
            .Returns(true);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<AlreadyRegisteredException>();
        ((AlreadyRegisteredException)exception).Code.ShouldBe("SignUpCommand.Email");
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingEmail_ShouldNotAddByRepository()
    {
        //arrange
        var command = new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            "first_name", "last_name");

        _readUserRepository
            .DoesEmailExistAsync(command.Email, default)
            .Returns(true);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        await _writeUserRepository
            .Received(0)
            .AddAsync(Arg.Any<User>(), default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingEmail_ShouldNotProcessedAnyEvents()
    {
        //arrange
        var command = new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            "first_name", "last_name");

        _readUserRepository
            .DoesEmailExistAsync(command.Email, default)
            .Returns(true);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command));

        //assert
        await _eventProcessor
            .Received(0)
            .PublishAsync(Arg.Any<IEvent>());
    }

    [Theory]
    [MemberData(nameof(GetInvalidSignUpCommand))]
    public async Task HandleAsync_GivenInvalidArguments_ShouldNotAddByRepository(SignUpCommand command)
    {
        //arrange
        _readUserRepository
            .DoesEmailExistAsync(command.Email, default)
            .Returns(false);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        await _writeUserRepository
            .Received(0)
            .AddAsync(Arg.Any<User>(), default);
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidSignUpCommand))]
    public async Task HandleAsync_GivenInvalidArguments_ShouldNotProcessAnyEvents(SignUpCommand command)
    {
        //arrange
        _readUserRepository
            .DoesEmailExistAsync(command.Email, default)
            .Returns(false);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command));

        //assert
        await _eventProcessor
            .Received(0)
            .PublishAsync(Arg.Any<IEvent>());
    }

    public static IEnumerable<object[]> GetInvalidSignUpCommand()
    {
        yield return
        [
            new SignUpCommand(UserId.New(), string.Empty, "Test123!",
                "test_first_name", "test_last_name")
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test_invalid_email", "Test123!",
                "test_first_name", "test_last_name")
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test@test.pl", "TEST123!",
                "test_first_name", "test_last_name")
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test@test.pl", "test123!",
                "test_first_name", "test_last_name")
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test@test.pl", "Test!",
                "test_first_name", "test_last_name")
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test@test.pl", "Test123",
                "test_first_name", "test_last_name")
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
                string.Empty, "test_last_name")
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
                new string('t', 1), "test_last_name")
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
                new string('t', 101), "test_last_name")
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
                "test_first_name", string.Empty)
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
                "test_first_name", new string('t', 1))
        ];

        yield return
        [
            new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
                "test_first_name", new string('t', 101))
        ];
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