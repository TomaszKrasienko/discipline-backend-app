using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.DailyProductivities;
using discipline.domain.ActivityRules.Entities;
using discipline.domain.ActivityRules.Repositories;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.domain.DailyProductivities.Entities;
using discipline.domain.DailyProductivities.Repositories;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.DailyProductivities.CreateActivityFromRule;

public sealed class CreateActivityFromRuleCommandHandlerTests
{
    private Task Act(CreateActivityFromRuleCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenDateForRuleAndExistingDailyProductivity_ShouldAddActivityAndUpdateDailyProductivityByRepository()
    {
        //arrange
        var activityRule = ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(), "My rule title", Mode.FirstDayOfMonth());
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var command = new CreateActivityFromRuleCommand(Guid.NewGuid(),activityRule.Id);

        _activityRuleRepository
            .GetByIdAsync(command.ActivityRuleId)
            .Returns(activityRule);

        _clock
            .DateNow()
            .Returns(now);

        var dailyProductivity = DailyProductivity.Create(DateOnly.FromDateTime(now), activityRule.UserId);
        _dailyProductivityRepository
            .GetByDateAsync(dailyProductivity.Day)
            .Returns(dailyProductivity);
        
        //act
        await Act(command);
        
        //assert
        dailyProductivity.Activities.Any(x => x.Id.Equals(command.ActivityId)).ShouldBeTrue();

        await _dailyProductivityRepository
            .Received(1)
            .UpdateAsync(dailyProductivity);
    }
    
    [Fact]
    public async Task HandleAsync_GivenDateForRuleAndNotExistingDailyProductivity_ShouldAddActivityAndAndDailyProductivityByRepository()
    {
        //arrange
        var activityRule = ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(), "My rule title", Mode.FirstDayOfMonth());
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var command = new CreateActivityFromRuleCommand(Guid.NewGuid(),activityRule.Id);

        _activityRuleRepository
            .GetByIdAsync(command.ActivityRuleId)
            .Returns(activityRule);

        _clock
            .DateNow()
            .Returns(now);
        
        //act
        await Act(command);
        
        //assert
        await _dailyProductivityRepository
            .Received(1)
            .AddAsync(Arg.Is<DailyProductivity>(arg
                => arg.Day.Value == DateOnly.FromDateTime(now)
                && arg.UserId.Value == activityRule.UserId.Value
                && arg.Activities.Any(x => x.Id.Equals(command.ActivityId))));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingRule_ShouldThrowActivityRuleNotFoundException()
    {
        //arrange
        var command = new CreateActivityFromRuleCommand(Guid.NewGuid(),Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<ActivityRuleNotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_GivenDateNotForRule_ShouldNotAddActivityAndUpdateDailyProductivityByRepository()
    {
        //arrange
        var activityRule = ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(), "My rule title", Mode.FirstDayOfMonth());
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 4);
        var command = new CreateActivityFromRuleCommand(Guid.NewGuid(),activityRule.Id);

        _activityRuleRepository
            .GetByIdAsync(command.ActivityRuleId)
            .Returns(activityRule);

        _clock
            .DateNow()
            .Returns(now);

        var dailyProductivity = DailyProductivity.Create(DateOnly.FromDateTime(now), activityRule.UserId);
        _dailyProductivityRepository
            .GetByDateAsync(dailyProductivity.Day)
            .Returns(dailyProductivity);
        
        //act
        await Act(command);
        
        //assert
        dailyProductivity.Activities.Any().ShouldBeFalse();

        await _dailyProductivityRepository
            .Received(1)
            .UpdateAsync(dailyProductivity);
    }
    

    #region arrange
    private readonly IClock _clock;
    private readonly IActivityRuleRepository _activityRuleRepository;
    private readonly IDailyProductivityRepository _dailyProductivityRepository;
    private readonly ICommandHandler<CreateActivityFromRuleCommand> _handler;
    
    public CreateActivityFromRuleCommandHandlerTests()
    {
        _clock = Substitute.For<IClock>();
        _activityRuleRepository = Substitute.For<IActivityRuleRepository>();
        _dailyProductivityRepository = Substitute.For<IDailyProductivityRepository>();
        _handler = new CreateActivityFromRuleCommandHandler(_clock, _activityRuleRepository,
            _dailyProductivityRepository);
    }
    #endregion
}