using discipline.application.Behaviours;
using discipline.application.Domain.ValueObjects.ActivityRules;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours;

public sealed class WeekdaysCheckingBehaviourTests
{
    private bool Act(DateTime dateTime, string mode) => _weekdayCheck.IsDateForMode(dateTime, mode);
    
    [Fact]
    public void IsDateForMode_GivenDateAndEveryDayMode_ShouldReturnTrue()
    {
        //arrange
        DateTime now = DateTime.Now;
        
        //act
        var result = Act(now, Mode.EveryDayMode());
        
        //assert
        result.ShouldBeTrue();
    } 
    
    [Fact]
    public void IsDateForMode_Given
    
    #region arrange
    private readonly IWeekdayCheck _weekdayCheck;

    public WeekdaysCheckingBehaviourTests()
    {
        _weekdayCheck = new WeekdayCheck();
    }
    #endregion
}