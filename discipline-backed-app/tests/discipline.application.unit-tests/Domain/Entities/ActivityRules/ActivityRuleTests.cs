using discipline.application.Domain.Entities;
using discipline.application.Domain.Exceptions;
using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Entities.ActivityRules;

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