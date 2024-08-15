using discipline.application.Behaviours;
using discipline.application.Features.DailyProductivities;
using discipline.domain.DailyProductivities.Exceptions;
using discipline.domain.DailyProductivities.Repositories;
using discipline.tests.shared.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.DailyProductivities.ChangeActivityCheck;

public sealed class ChangeActivityCheckCommandHandlerTests
{
    private Task Act(ChangeActivityCheckCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenExistingDailyProductivityForActivityId_ShouldUpdateDailyProductivityAfterChangeActivityCheck()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        var isChecked = activity.IsChecked.Value;
        var command = new ChangeActivityCheckCommand(activity.Id);

        _dailyProductivityRepository
            .GetByActivityId(command.ActivityId)
            .Returns(dailyProductivity);
        
        //act
        await Act(command);
        
        //assert
        var updatedActivity = dailyProductivity.Activities.First(x => x.Id.Equals(activity.Id));
        updatedActivity.IsChecked.Value.ShouldBe(!isChecked);

        await _dailyProductivityRepository
            .Received(1)
            .UpdateAsync(dailyProductivity);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingDailyProductivityForActivityId_ShouldThrowActivityNotFoundException()
    {
        //arrange
        var command = new ChangeActivityCheckCommand(Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<ActivityNotFoundException>();
    }
    
    #region arrange
    private readonly IDailyProductivityRepository _dailyProductivityRepository;
    private readonly ICommandHandler<ChangeActivityCheckCommand> _handler;

    public ChangeActivityCheckCommandHandlerTests()
    {
        _dailyProductivityRepository = Substitute.For<IDailyProductivityRepository>();
        _handler = new ChangeActivityCheckCommandHandler(_dailyProductivityRepository);
    }
    #endregion
}