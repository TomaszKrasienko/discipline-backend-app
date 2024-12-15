using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.application.ActivityRules.Events;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Events;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Events;
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
            new ActivityRuleDetailsSpecification("test_title", "test_note"), Mode.CustomMode, [1], 
            [new StageSpecification("test_stage_title", 1)]);
        
        _readWriteActivityRuleRepository
            .ExistsAsync(command.Details.Title, command.UserId, default)
            .Returns(false);

        //act
        await Act(command);
        
        //assert
        await _readWriteActivityRuleRepository
            .Received(1)
            .AddAsync(Arg.Is<ActivityRule>(arg
                => arg.Id == command.Id
                   && arg.Details.Title == command.Details.Title
                   && arg.Details.Note == command.Details.Note
                   && arg.Mode.Value == command.Mode));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingTitle_ShouldSendIntegrationEvent()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            new ActivityRuleDetailsSpecification("test_title", "test_note"), Mode.CustomMode, [1], 
            [new StageSpecification("test_stage_title", 1)]);
        
        _readWriteActivityRuleRepository
            .ExistsAsync(command.Details.Title, command.UserId, default)
            .Returns(false);

        //act
        await Act(command);
        
        //assert
        await _eventProcessor
            .Received(1)
            .PublishAsync(Arg.Is<ActivityRuleRegistered>(arg
                => arg.ActivityRuleId == command.Id
                   && arg.UserId == command.UserId));
    }
    
    [Fact]
    public async Task HandleAsync_GivenAlreadyRegisteredRuleTitle_ShouldThrowAlreadyRegisteredExceptionWithCode()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            new ActivityRuleDetailsSpecification("Rule title", "Rule note"),
            Mode.EveryDayMode, null, null);
        _readWriteActivityRuleRepository
            .ExistsAsync(command.Details.Title, command.UserId, default)
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
       var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification(
           "Rule title", "Rule note"), Mode.EveryDayMode, null, null);
       _readWriteActivityRuleRepository
            .ExistsAsync(command.Details.Title, command.UserId, default)
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
            .ExistsAsync(command.Details.Title, command.UserId, default)
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
    private readonly IEventProcessor _eventProcessor;
    private readonly ICommandHandler<CreateActivityRuleCommand> _handler;

    public CreateActivityRuleCommandHandlerTests()
    {
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _handler = new CreateActivityRuleCommandHandler(_readWriteActivityRuleRepository,
            _eventProcessor);
    }
    #endregion
}