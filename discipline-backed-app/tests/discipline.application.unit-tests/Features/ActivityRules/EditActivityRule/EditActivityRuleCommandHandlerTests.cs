using discipline.application.Domain.Repositories;
using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.application.Exceptions;
using discipline.application.Features.ActivityRules;
using discipline.application.Features.Configuration.Base.Abstractions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.ActivityRules.EditActivityRule;

public sealed class EditActivityRuleCommandHandlerTests
{
    private Task Act(EditActivityRuleCommand command) => _handler.HandleAsync(command, default);

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