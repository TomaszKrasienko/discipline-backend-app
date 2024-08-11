using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.ActivityRules;
using discipline.domain.ActivityRules.Repositories;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.tests.shared.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.ActivityRules.EditActivityRule;

public sealed class EditActivityRuleCommandHandlerTests
{
    private Task Act(EditActivityRuleCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingActivityRule_ShouldUpdateAndUpdateByRepository()
    {
        //arrange
        var activityRule = ActivityRuleFactory.Get();
        var command = new EditActivityRuleCommand(activityRule.Id, "New title", Mode.EveryDayMode(), null);

        _activityRuleRepository
            .GetByIdAsync(command.Id)
            .Returns(activityRule);
        
        //act
        await Act(command);
        
        //assert
        activityRule.Title.Value.ShouldBe(command.Title);
        activityRule.Mode.Value.ShouldBe(command.Mode);
        activityRule.SelectedDays.ShouldBeNull();

        await _activityRuleRepository
            .Received(1)
            .UpdateAsync(activityRule);

    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityRule_ShouldThrowActivityRuleNotFoundException()
    {
        //arrange
        var command = new EditActivityRuleCommand(Guid.NewGuid(), "Title", Mode.EveryDayMode(),
            null);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<ActivityRuleNotFoundException>();
    }
    
    #region arrange
    private readonly IActivityRuleRepository _activityRuleRepository;
    private readonly ICommandHandler<EditActivityRuleCommand> _handler;

    public EditActivityRuleCommandHandlerTests()
    {
        _activityRuleRepository = Substitute.For<IActivityRuleRepository>();
        _handler = new EditActivityRuleCommandHandler(_activityRuleRepository);
    }
    #endregion
}