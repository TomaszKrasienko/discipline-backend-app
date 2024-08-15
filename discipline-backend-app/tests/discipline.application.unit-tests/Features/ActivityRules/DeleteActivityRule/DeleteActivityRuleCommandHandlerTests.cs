using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.ActivityRules;
using discipline.domain.ActivityRules.Repositories;
using discipline.tests.shared.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.ActivityRules.DeleteActivityRule;

public sealed class DeleteActivityRuleCommandHandlerTests
{
    private Task Act(DeleteActivityRuleCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingActivityRule_ShouldDeleteActivityRyleByRepository()
    {
        //arrange
        var activityRule = ActivityRuleFactory.Get();
        var command = new DeleteActivityRuleCommand(activityRule.Id);
        _activityRuleRepository
            .GetByIdAsync(command.Id)
            .Returns(activityRule);
        
        //act
        await Act(command);
        
        //assert
        await _activityRuleRepository
            .Received(1)
            .DeleteAsync(activityRule);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityRule_ShouldThrowActivityRuleNotFoundException()
    {
        //arrange
        var command = new DeleteActivityRuleCommand(Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<ActivityRuleNotFoundException>();
    }
    
    #region arrange
    private readonly IActivityRuleRepository _activityRuleRepository;
    private readonly ICommandHandler<DeleteActivityRuleCommand> _handler;

    public DeleteActivityRuleCommandHandlerTests()
    {
        _activityRuleRepository = Substitute.For<IActivityRuleRepository>();
        _handler = new DeleteActivityRuleCommandHandler(_activityRuleRepository);
    }
    #endregion
}