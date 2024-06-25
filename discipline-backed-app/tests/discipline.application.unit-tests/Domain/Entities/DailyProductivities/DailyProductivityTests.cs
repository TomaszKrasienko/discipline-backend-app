using discipline.application.Domain.Exceptions;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Entities.DailyProductivities;

public sealed class DailyProductivityTests
{
    [Fact]
    public void AddActivity_GivenNotExistedTitle_ShouldAddToActivities()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var id = Guid.NewGuid();
        var title = "title";
        //act
        dailyProductivity.AddActivity(id, title);
        
        //assert
        dailyProductivity
            .Activities
            .Any(x => x.Id.Equals(id) && x.Title == title)
            .ShouldBeTrue();
    }
    
    [Fact]
    public void AddActivity_GivenAlreadyExistingTitle_ShouldThrowActivityTitleAlreadyRegisteredException()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var title = "Activity title";
        dailyProductivity.AddActivity(Guid.NewGuid(), title);
        
        //act
        var exception = Record.Exception(() => dailyProductivity.AddActivity(Guid.NewGuid(), title));
        
        //assert
        exception.ShouldBeOfType<ActivityTitleAlreadyRegisteredException>();
    }

    [Fact]
    public void DeleteActivity_GivenExistingActivity_ShouldRemoveActivityFromList()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);

        //act
        dailyProductivity.DeleteActivity(activity.Id);
        
        //assert
        dailyProductivity.Activities.Any(x => x.Id.Equals(activity.Id)).ShouldBeFalse();
    }

    [Fact]
    public void DeleteActivity_GivenNotExistingActivity_ShouldThrowActivityNotFoundException()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        
        //act
        var exception = Record.Exception(() => dailyProductivity.DeleteActivity(Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<ActivityNotFoundException>();
    }
    
    [Fact]
    public void ChangeActivityCheck_GivenExistingActivity_ChangeActivityCheck()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        var isChecked = activity.IsChecked.Value;
        
        //act
        dailyProductivity.ChangeActivityCheck(activity.Id);
        
        //assert
        var updatedActivity = dailyProductivity.Activities.First(x => x.Id.Equals(activity.Id));
        updatedActivity.IsChecked.Value.ShouldBe(!isChecked);
    }

    [Fact]
    public void ChangeActivityCheck_GivenNotExistingActivity_ShouldThrowActivityNotFoundException()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        
        //act
        var exception = Record.Exception(() => dailyProductivity.ChangeActivityCheck(Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<ActivityNotFoundException>();
    }
}