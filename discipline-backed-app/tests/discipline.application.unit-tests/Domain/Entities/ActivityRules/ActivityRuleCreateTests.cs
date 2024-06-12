using discipline.application.Domain.Entities;
using discipline.application.Domain.Exceptions;
using discipline.application.Domain.ValueObjects.ActivityRules;
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
        result.SelectedDays[0].Value.ShouldBe(selectedDays[0]);
        result.SelectedDays[1].Value.ShouldBe(selectedDays[1]);
        result.SelectedDays[2].Value.ShouldBe(selectedDays[2]);
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