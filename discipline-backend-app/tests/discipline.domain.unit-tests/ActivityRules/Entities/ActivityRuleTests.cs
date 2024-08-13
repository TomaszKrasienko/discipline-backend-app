using discipline.domain.ActivityRules.Exceptions;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.ActivityRules.Entities;

public sealed class ActivityRuleTests
{
    [Fact]
    public void Edit_GivenWithWithoutSelectedDaysAndValidMode_ShouldUpdateFields()
    {
        //arrange
        var activityRule = ActivityRuleFactory.Get([1]);
        var title = "NewTitle";
        var mode = Mode.EveryDayMode();
        
        //act
        activityRule.Edit(title, mode);
        
        //assert
        activityRule.Title.Value.ShouldBe(title);
        activityRule.Mode.Value.ShouldBe(mode);
        activityRule.SelectedDays.ShouldBeNull();
    }
    
    [Fact]
    public void Edit_GivenWithWithSelectedDaysAndValidMode_ShouldUpdateFields()
    {
        //arrange
        var activityRule = ActivityRuleFactory.Get();
        var title = "NewTitle";
        var mode = Mode.CustomMode();
        List<int> selectedDays = [1, 3, 5];
        //act
        activityRule.Edit(title, mode, selectedDays);
        
        //assert
        activityRule.Title.Value.ShouldBe(title);
        activityRule.Mode.Value.ShouldBe(mode);
        activityRule.SelectedDays.Select(x => x.Value).Contains(selectedDays[0]).ShouldBeTrue();
        activityRule.SelectedDays.Select(x => x.Value).Contains(selectedDays[1]).ShouldBeTrue();
        activityRule.SelectedDays.Select(x => x.Value).Contains(selectedDays[2]).ShouldBeTrue();
    }
    
    [Fact]
    public void Edit_GivenNotCustomModeAndSelectedDays_ShouldThrowInvalidModeForSelectedDaysException()
    {
        //arrange
        var activityRule = ActivityRuleFactory.Get([1]);
        
        //act
        var exception = Record.Exception(() => activityRule.Edit("New title", Mode.EveryDayMode(), [1]));
        
        //assert
        exception.ShouldBeOfType<InvalidModeForSelectedDaysException>();
    }
}