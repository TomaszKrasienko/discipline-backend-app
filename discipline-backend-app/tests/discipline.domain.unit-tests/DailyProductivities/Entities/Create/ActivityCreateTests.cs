using discipline.domain.ActivityRules.Entities;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.domain.DailyProductivities.Entities;
using discipline.domain.DailyProductivities.Exceptions;
using discipline.domain.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.DailyProductivities.Entities.Create;

public sealed class ActivityCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnActivityWithFilledFields()
    {
        //arrange
        var id = ActivityId.New();
        var title = "Activity title";
        
        //act
        var result = Activity.Create(id, title);
        
        //assert
        result.Id.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
    }
    
    [Fact]
    public void CreateFromRule_GivenModeForDay_ShouldReturnActivity()
    {
        //arrange
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var mode = Mode.FirstDayOfMonth();
        var id = ActivityId.New();
        
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), UserId.New(),  "Title", mode);
        
        //act
        var result = Activity.CreateFromRule(id, now, activityRule);

        //assert
        result.Id.ShouldBe(id);
        result.Title.Value.ShouldBe(activityRule.Title);
        result.IsChecked.Value.ShouldBeFalse();
        result.ParentRuleId.Value.ShouldBe(activityRule.Id.Value);
    }

    [Fact]
    public void CreateFromRule_GivenModeNotForDay_ShouldReturnNull()
    {
        //arrange
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 4);
        var mode = Mode.FirstDayOfMonth();
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "Title", mode);
        
        //act
        var result = Activity.CreateFromRule(ActivityId.New(), now, activityRule);

        //assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyActivityTitleException()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(ActivityId.New(), string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyActivityTitleException>();
    }
    
    [Fact]
    public void Create_GivenTitleWithInvalidLength_ShouldThrowEmptyActivityTitleException()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(ActivityId.New(), "T"));
        
        //assert
        exception.ShouldBeOfType<InvalidActivityTitleLengthException>();
    }
}