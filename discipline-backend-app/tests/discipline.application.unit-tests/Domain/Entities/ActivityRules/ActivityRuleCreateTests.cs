using Bogus.DataSets;
using discipline.application.Domain.Entities;
using discipline.application.Domain.Exceptions;
using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Entities.ActivityRules;

public sealed class ActivityRuleCreateTests
{
    [Fact]
    public void Create_GivenWithoutSelectedDaysWithValidMode_ShouldReturnActivityRuleWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Test title";
        var mode = Mode.EveryDayMode();
        
        //act
        var result = ActivityRule.Create(id, title, mode);
        
        //assert
        result.ShouldBeOfType<ActivityRule>();
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Mode.Value.ShouldBe(mode);
    }
    
    [Fact]
    public void Create_GivenWithSelectedDaysWithValidMode_ShouldReturnActivityRuleWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Test title";
        var mode = Mode.CustomMode();
        List<int> selectedDays = [1, 2, 4];
        
        //act
        var result = ActivityRule.Create(id, title, mode, selectedDays);
        
        //assert
        result.ShouldBeOfType<ActivityRule>();
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Mode.Value.ShouldBe(mode);
        result.SelectedDays.Select(x => x.Value).Contains(selectedDays[0]).ShouldBeTrue();
        result.SelectedDays.Select(x => x.Value).Contains(selectedDays[1]).ShouldBeTrue();
        result.SelectedDays.Select(x => x.Value).Contains(selectedDays[2]).ShouldBeTrue();
    }
    
    [Fact]
    public void CreateFromRule_GivenModeForDay_ShouldReturnActivity()
    {
        //arrange
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var mode = Mode.FirstDayOfMonth();
        var id = Guid.NewGuid();
        var activityRule = ActivityRule.Create(Guid.NewGuid(), "Title", mode);
        
        //act
        var result = Activity.CreateFromRule(id, now, activityRule);

        //assert
        result.Id.Value.ShouldBe(id);
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
        var activityRule = ActivityRule.Create(Guid.NewGuid(), "Title", mode);
        
        //act
        var result = Activity.CreateFromRule(Guid.NewGuid(), now, activityRule);

        //assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyActivityRuleTitleException()
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), string.Empty,
            Mode.EveryDayMode(), null));
        
        //assert
        exception.ShouldBeOfType<EmptyActivityRuleTitleException>();
    }
    
    [Fact]
    public void Create_GivenTitleWithInvalidLength_ShouldThrowEmptyActivityRuleTitleException()
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), "T",
            Mode.EveryDayMode(), null));
        
        //assert
        exception.ShouldBeOfType<InvalidActivityRuleTitleLengthException>();
    }

    [Fact]
    public void Create_GivenEmptyMode_ShouldThrowEmptyActivityRuleModeException()
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), "Test title",
            string.Empty, null));
        
        //assert
        exception.ShouldBeOfType<EmptyActivityRuleModeException>();
    }
    
    [Fact]
    public void Create_GivenRandomMode_ShouldThrowModeUnavailableException()
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), "Test title",
            Guid.NewGuid().ToString(), null));
        
        //assert
        exception.ShouldBeOfType<ModeUnavailableException>();
    }

    [Fact]
    public void Create_GivenSelectedDaysNotEmptyAndModeOtherThanCustom_ShouldThrowInvalidModeForSelectedDaysException()
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), "Test title",
            Mode.EveryDayMode(), [1, 2, 3]));
        
        //assert
        exception.ShouldBeOfType<InvalidModeForSelectedDaysException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10)]
    public void Create_GivenInvalidSelectedDay_ShouldThrowSelectedDayOutOfRangeException(int selectedDay)
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), "Test title",
            Mode.CustomMode(), [selectedDay]));
        
        //assert
        exception.ShouldBeOfType<SelectedDayOutOfRangeException>();
    }
}