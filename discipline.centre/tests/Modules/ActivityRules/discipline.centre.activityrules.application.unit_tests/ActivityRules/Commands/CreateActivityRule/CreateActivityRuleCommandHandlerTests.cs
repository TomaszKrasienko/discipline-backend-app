using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.CreateActivityRule;

public partial class CreateActivityRuleCommandHandlerTests
{
    private Task Act(CreateActivityRuleCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingTitle_ShouldCreateActivityRule()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Rule title", Mode.EveryDayMode, null);
        
        _readWriteActivityRuleRepository
            .ExistsAsync(command.Title, command.UserId, default)
            .Returns(false);

        //act
        await Act(command);
        
        //assert
        await _readWriteActivityRuleRepository
            .Received(1)
            .AddAsync(Arg.Is<ActivityRule>(arg
                => arg.Id == command.Id
                   && arg.Title.Value == command.Title
                   && arg.Mode.Value == command.Mode));
    }
    
    [Fact]
    public async Task HandleAsync_GivenAlreadyRegisteredRuleTitle_ShouldThrowAlreadyRegisteredExceptionWithCode()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Rule title", Mode.EveryDayMode, null);
        _readWriteActivityRuleRepository
            .ExistsAsync(command.Title, command.UserId, default)
            .Returns(true);

        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<AlreadyRegisteredException>();
        ((AlreadyRegisteredException)exception).Code.ShouldBe("CreateActivityRule.Title");
    }
    
    [Fact]
    public async Task HandleAsync_GivenAlreadyRegisteredRuleTitle_ShouldNotAddAnyActivityRuleByRepository()
    {
        //arrange
       var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Rule title", Mode.EveryDayMode, null);
       _readWriteActivityRuleRepository
            .ExistsAsync(command.Title, command.UserId, default)
            .Returns(true);

        //act
        await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        await _readWriteActivityRuleRepository
            .Received(0)
            .AddAsync(Arg.Any<ActivityRule>(), default);
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateActivityRuleCommand))]
    public async Task HandleAsync_GivenInvalidArgumentsForActivityRule_ShouldNotAddActivityRule(CreateActivityRuleCommand command)
    {
        //arrange
        _readWriteActivityRuleRepository
            .ExistsAsync(command.Title, command.UserId, default)
            .Returns(false);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        await _readWriteActivityRuleRepository
            .Received(0)
            .AddAsync(Arg.Any<ActivityRule>(), default);
    }
    
    #region arrange
    private readonly IReadWriteActivityRuleRepository _readWriteActivityRuleRepository;
    private readonly ICommandHandler<CreateActivityRuleCommand> _handler;

    public CreateActivityRuleCommandHandlerTests()
    {
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _handler = new CreateActivityRuleCommandHandler(_readWriteActivityRuleRepository);
    }
    #endregion
}