using discipline.domain.ActivityRules.Entities;
using discipline.domain.ActivityRules.Exceptions;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.ActivityRules.Entities.Create;

public sealed class ActivityRuleCreateTests
{
    [Fact]
    public void Create_GivenWithoutSelectedDaysWithValidMode_ShouldReturnActivityRuleWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var title = "Test title";
        var mode = Mode.EveryDayMode();
        
        //act
        var result = ActivityRule.Create(id, userId, title, mode);
        
        //assert
        result.ShouldBeOfType<ActivityRule>();
        result.Id.Value.ShouldBe(id);
        result.UserId.Value.ShouldBe(userId);
        result.Title.Value.ShouldBe(title);
        result.Mode.Value.ShouldBe(mode);
    }
    
    [Fact]
    public void Create_GivenWithSelectedDaysWithValidMode_ShouldReturnActivityRuleWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var title = "Test title";
        var mode = Mode.CustomMode();
        List<int> selectedDays = [1, 2, 4];
        
        //act
        var result = ActivityRule.Create(id, userId, title, mode, selectedDays);
        
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
    public void Create_GivenEmptyTitle_ShouldThrowEmptyActivityRuleTitleException()
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(),string.Empty,
            Mode.EveryDayMode(), null));
        
        //assert
        exception.ShouldBeOfType<EmptyActivityRuleTitleException>();
    }
    
    [Fact]
    public void Create_GivenTitleWithInvalidLength_ShouldThrowEmptyActivityRuleTitleException()
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(),"T",
            Mode.EveryDayMode(), null));
        
        //assert
        exception.ShouldBeOfType<InvalidActivityRuleTitleLengthException>();
    }

    [Fact]
    public void Create_GivenEmptyMode_ShouldThrowEmptyActivityRuleModeException()
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(),"Test title",
            string.Empty, null));
        
        //assert
        exception.ShouldBeOfType<EmptyActivityRuleModeException>();
    }
    
    [Fact]
    public void Create_GivenRandomMode_ShouldThrowModeUnavailableException()
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(),"Test title",
            Guid.NewGuid().ToString(), null));
        
        //assert
        exception.ShouldBeOfType<ModeUnavailableException>();
    }

    [Fact]
    public void Create_GivenSelectedDaysNotEmptyAndModeOtherThanCustom_ShouldThrowInvalidModeForSelectedDaysException()
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(), "Test title",
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
        var exception = Record.Exception(() => ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(),"Test title",
            Mode.CustomMode(), [selectedDay]));
        
        //assert
        exception.ShouldBeOfType<SelectedDayOutOfRangeException>();
    }
}