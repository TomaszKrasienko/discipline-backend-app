using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.DeleteActivityRule;

public sealed class DeleteActivityRuleCommandHandlerTests
{
    private Task Act(DeleteActivityRuleCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingActivityRule_ShouldDeleteActivityRuleByRepository()
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();

        _readWriteActivityRuleRepository
            .GetByIdAsync(activityRule.Id, activityRule.UserId)
            .Returns(activityRule);

        var command = new DeleteActivityRuleCommand(activityRule.Id, activityRule.UserId);
        
        //act
        await Act(command);
        
        //aseert
        await _readWriteActivityRuleRepository
            .Received(1)
            .DeleteAsync(activityRule);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityRule_ShouldNotAttemptsToDeleteByRepository()
    {
        //arrange
        var command = new DeleteActivityRuleCommand(ActivityRuleId.New(), UserId.New());

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.UserId)
            .ReturnsNull();
        
        //act
        await Act(command);
        
        //assert
        await _readWriteActivityRuleRepository
            .Received(0)
            .DeleteAsync(Arg.Any<ActivityRule>(), default);
    }
    
    #region arrange
    private readonly IReadWriteActivityRuleRepository _readWriteActivityRuleRepository;
    private readonly ICommandHandler<DeleteActivityRuleCommand> _handler;
    
    public DeleteActivityRuleCommandHandlerTests()
    {
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _handler = new DeleteActivityRuleCommandHandler(_readWriteActivityRuleRepository);
    }
    #endregion
}