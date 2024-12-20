using discipline.application.Behaviours;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Exceptions;
using discipline.application.Features.ActivityRules;
using discipline.domain.ActivityRules.Entities;
using discipline.domain.ActivityRules.Repositories;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.domain.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.ActivityRules.CreateActivityRule;

public sealed class CreateActivityRuleCommandHandlerTests
{
    private Task Act(CreateActivityRuleCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingTitle_ShouldCreateActivityRule()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Rule title", Mode.EveryDayMode(), null);
        _activityRuleRepository
            .ExistsAsync(command.Title, default)
            .Returns(false);

        //act
        await Act(command);
        
        //assert
        await _activityRuleRepository
            .Received(1)
            .AddAsync(Arg.Is<ActivityRule>(arg
                => arg.Id == command.Id
                   && arg.Title.Value == command.Title
                   && arg.Mode.Value == command.Mode));
    }
    
    [Fact]
    public async Task HandleAsync_GivenAlreadyRegisteredRuleTitle_ShouldThrowActivityRuleTitleAlreadyRegisteredException()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Rule title", Mode.EveryDayMode(), null);
        _activityRuleRepository
            .ExistsAsync(command.Title, default)
            .Returns(true);

        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<ActivityRuleTitleAlreadyRegisteredException>();
    }
    
    #region arrange
    private readonly IActivityRuleRepository _activityRuleRepository;
    private readonly ICommandHandler<CreateActivityRuleCommand> _handler;

    public CreateActivityRuleCommandHandlerTests()
    {
        _activityRuleRepository = Substitute.For<IActivityRuleRepository>();
        _handler = new CreateActivityRuleCommandHandler(_activityRuleRepository);
    }
    #endregion
}