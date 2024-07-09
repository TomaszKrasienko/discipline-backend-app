using discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.application.Domain.DailyProductivities.Services.Abstractions;
using discipline.application.Domain.DailyProductivities.Services.Internal;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Services;

public sealed class WeekdayCheckServiceTests
{
    private bool Act(DateTime now, string mode, List<int>? selectedDays = null) 
        => _weekdayCheckService.IsDateForMode(now, mode, selectedDays);

    [Fact]
    public void IsDateForMode_GivenEveryDayMode_ShouldReturnTrue()
    {
        //arrange
        var now = DateTime.Now;
        var mode = Mode.EveryDayMode();
        
        //act
        var result = Act(now, mode);
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void IsDateForMode_GivenFirstDayOfWeekModeAndDateFirstDayOfWeek_ShouldReturnTrue()
    {
        //arrange
        var startDay = DateTime.Now;
        int daysToAdd = ((int)DayOfWeek.Monday - (int)startDay.DayOfWeek);
        var mondayDate = startDay.AddDays(daysToAdd == 0 ? 7 : daysToAdd);
        
        //act
        var result = Act(mondayDate, Mode.FirstDayOfWeekMode());
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void IsDateForMode_GivenFirstDayOfWeekModeAndDateNotFirstDayOfWeek_ShouldReturnTrue()
    {
        //arrange
        var startDay = DateTime.Now;
        var notMondayDate =
            startDay.DayOfWeek == DayOfWeek.Monday
                ? startDay.AddDays(1)
                : startDay; 
        
        //act
        var result = Act(notMondayDate, Mode.FirstDayOfWeekMode());
        
        //assert
        result.ShouldBeFalse();
    }
    
    [Fact]
    public void IsDateForMode_GivenLastDayOfWeekModeAndDateLastDayOfWeek_ShouldReturnTrue()
    {
        //arrange
        var startDay = DateTime.Now;
        int daysToAdd = ((int)DayOfWeek.Sunday - (int)startDay.DayOfWeek);
        var mondayDate = startDay.AddDays(daysToAdd == 0 ? 7 : daysToAdd);
        
        //act
        var result = Act(mondayDate, Mode.LastDayOfWeekMode());
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void IsDateForMode_GivenLastDayOfWeekModeAndDateNotLastDayOfWeek_ShouldReturnTrue()
    {
        //arrange
        var startDay = DateTime.Now;
        var notMondayDate =
            startDay.DayOfWeek == DayOfWeek.Sunday
                ? startDay.AddDays(1)
                : startDay; 
        
        //act
        var result = Act(notMondayDate, Mode.LastDayOfWeekMode());
        
        //assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsDateForMode_GivenFirstDayOffMonthModeAndDateFirstDayOffMonth_ShouldReturnTrue()
    {
        //arrange
        var startDay = DateTime.Now;
        var firstDayOfMonthDate = new DateTime(startDay.Year, startDay.Month, 1);
        
        //act
        var result = Act(firstDayOfMonthDate, Mode.FirstDayOfMonth());
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void IsDateForMode_GivenFirstDayOffMonthModeAndDateNotFirstDayOffMonth_ShouldReturnTrue()
    {
        //arrange
        var startDay = DateTime.Now;
        var notFirstDayOfMonthDate = startDay.Day == 1 ? startDay.AddDays(1) : startDay;
        
        //act
        var result = Act(notFirstDayOfMonthDate, Mode.FirstDayOfMonth());
        
        //assert
        result.ShouldBeFalse();
    }
    
    [Fact]
    public void IsDateForMode_GivenLastDayOffMonthModeAndDateLastDayOfMonth_ShouldReturnTrue()
    {
        //arrange
        var startDay = DateTime.Now;
        var lastDayOfMonthDate = new DateTime(startDay.Year, startDay.Month,
            DateTime.DaysInMonth(startDay.Year, startDay.Month));
        
        //act
        var result = Act(lastDayOfMonthDate, Mode.LastDayOfMonthMode());
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void IsDateForMode_GivenLastDayOfMonthModeAndDateNotLastDayOffMonth_ShouldReturnTrue()
    {
        //arrange
        var startDay = DateTime.Now;
        var notLastDayOfMonthDate = startDay.Day == DateTime.DaysInMonth(startDay.Year, startDay.Month)
            ? startDay.AddDays(-1)
            : startDay;
        
        //act
        var result = Act(notLastDayOfMonthDate, Mode.LastDayOfMonthMode());
        
        //assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsDateForMode_GivenCustomModeAndSelectedDaysWithValidDay_ShouldReturnTrue()
    {
        //arrange
        var now = DateTime.Now;
        List<int> selectedDays = [(int)now.DayOfWeek];
        
        //act
        var result = Act(now, Mode.CustomMode(), selectedDays);
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void IsDateForMode_GivenCustomModeAndSelectedDaysWithInvalidDay_ShouldReturnTrue()
    {
        //arrange
        var now = DateTime.Now;
        List<int> allDays = [0, 1, 2, 3, 4, 5, 6];
        allDays.Remove((int)now.DayOfWeek);
        
        List<int> selectedDays = allDays;
        
        //act
        var result = Act(now, Mode.CustomMode(), selectedDays);
        
        //assert
        result.ShouldBeFalse();
    }
    
    #region arrange
    private readonly IWeekdayCheckService _weekdayCheckService;

    public WeekdayCheckServiceTests()
        => _weekdayCheckService = new WeekdayCheckService();
    
    #endregion
}