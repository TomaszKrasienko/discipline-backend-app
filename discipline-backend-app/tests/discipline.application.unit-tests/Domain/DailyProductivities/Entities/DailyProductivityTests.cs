using discipline.application.Domain.ActivityRules.Entities;
using discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.application.Domain.DailyProductivities.Exceptions;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.DailyProductivities.Entities;

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
    public void AddActivityFromRule_GivenNotExistedTitleAndNotNullActivity_ShouldAddToActivities()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activityRule = ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(), "activity title",
            Mode.EveryDayMode());
        var id = Guid.NewGuid();
        
        //act
        dailyProductivity.AddActivityFromRule(id, DateTime.Now, activityRule);
    
        //assert
        dailyProductivity
            .Activities
            .Any(x 
                => x.Id.Equals(id) 
                && x.Title == activityRule.Title
                && x.ParentRuleId.Equals(activityRule.Id))
            .ShouldBeTrue();
    }
    
    [Fact]
    public void AddActivityFromRule_GivenNotExistedTitleAndNullActivity_ShouldAddToActivities()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activityRule = ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(),"activity title",
            Mode.FirstDayOfMonth());
        var id = Guid.NewGuid();
        
        //act
        dailyProductivity.AddActivityFromRule(id, new DateTime(2024, 06, 2), activityRule);
    
        //assert
        dailyProductivity
            .Activities
            .Any(x 
                => x.Id.Equals(id) 
                   && x.Title == activityRule.Title
                   && x.ParentRuleId.Equals(activityRule.Id))
            .ShouldBeFalse();
    }
    
    [Fact]
    public void AddActivityFromRule_GivenAlreadyExistingTitle_ShouldThrowActivityTitleAlreadyRegisteredException()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var title = "Activity title";
        dailyProductivity.AddActivity(Guid.NewGuid(), title);
        
        var activityRule = ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(), title,
            Mode.EveryDayMode());
    
        //act
        var exception = Record.Exception(() => dailyProductivity.AddActivityFromRule(Guid.NewGuid(), 
            DateTime.Now, activityRule));
    
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