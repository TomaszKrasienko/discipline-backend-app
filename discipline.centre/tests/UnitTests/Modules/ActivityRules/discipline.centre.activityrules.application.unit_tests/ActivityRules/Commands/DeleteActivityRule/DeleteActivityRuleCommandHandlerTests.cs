using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.application.ActivityRules.Events;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.DeleteActivityRule;

public sealed class DeleteActivityRuleCommandHandlerTests
{
    private Task Act(DeleteActivityRuleCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenExistingActivityRule_WhenHandleAsync_ThenShouldDeleteActivityRuleByRepository()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();

        _readWriteActivityRuleRepository
            .GetByIdAsync(activityRule.Id, activityRule.UserId)
            .Returns(activityRule);

        var command = new DeleteActivityRuleCommand(activityRule.UserId, activityRule.Id);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteActivityRuleRepository
            .Received(1)
            .DeleteAsync(activityRule);
    }
    
    [Fact]
    public async Task GivenExistingActivityRule_WhenHandleAsync_ThenShouldSendActivityRuleRemovedEvent()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();

        _readWriteActivityRuleRepository
            .GetByIdAsync(activityRule.Id, activityRule.UserId)
            .Returns(activityRule);

        var command = new DeleteActivityRuleCommand(activityRule.UserId, activityRule.Id);
        
        // Act
        await Act(command);
        
        // Assert
        await _eventProcessor
            .Received(1)
            .PublishAsync(Arg.Is<ActivityRuleDeleted>(arg
                => arg.UserId == activityRule.UserId.Value
                && arg.ActivityRuleId == activityRule.Id.Value));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityRule_ShouldNotAttemptsToDeleteByRepository()
    {
        //arrange
        var command = new DeleteActivityRuleCommand(UserId.New(), ActivityRuleId.New());

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.UserId)
            .ReturnsNull();
        
        //act
        await Act(command);
        
        //assert
        await _readWriteActivityRuleRepository
            .Received(0)
            .DeleteAsync(Arg.Any<ActivityRule>(), CancellationToken.None);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityRule_ShouldNotAttemptsToSendActivityRuleRemovedEvent()
    {
        //arrange
        var command = new DeleteActivityRuleCommand(UserId.New(), ActivityRuleId.New());

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.UserId)
            .ReturnsNull();
        
        //act
        await Act(command);
        
        //assert
        await _eventProcessor
            .Received(0)
            .PublishAsync(Arg.Any<ActivityRuleDeleted>());
    }
    
    #region arrange
    private readonly IReadWriteActivityRuleRepository _readWriteActivityRuleRepository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICommandHandler<DeleteActivityRuleCommand> _handler;
    
    public DeleteActivityRuleCommandHandlerTests()
    {
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _handler = new DeleteActivityRuleCommandHandler(_readWriteActivityRuleRepository,
            _eventProcessor);
    }
    #endregion
}