using discipline.domain.DailyProductivities.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.DailyProductivities.Entities.Create;

public sealed class ActivityCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnActivityWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Activity title";
        
        //act
        var result = Activity.Create(id, title);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
    }
    
    [Fact]
    public void CreateFromRule_GivenModeForDay_ShouldReturnActivity()
    {
        //arrange
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var mode = Mode.FirstDayOfMonth();
        var id = Guid.NewGuid();
        
        var activityRule = ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(),  "Title", mode);
        
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
        var activityRule = ActivityRule.Create(Guid.NewGuid(), Guid.NewGuid(), "Title", mode);
        
        //act
        var result = Activity.CreateFromRule(Guid.NewGuid(), now, activityRule);

        //assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyActivityTitleException()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(Guid.NewGuid(), string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyActivityTitleException>();
    }
    
    [Fact]
    public void Create_GivenTitleWithInvalidLength_ShouldThrowEmptyActivityTitleException()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(Guid.NewGuid(), "T"));
        
        //assert
        exception.ShouldBeOfType<InvalidActivityTitleLengthException>();
    }
}