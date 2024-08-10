using discipline.domain.UsersCalendars.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.UsersCalendars.Policies;

public sealed class MeetingTimePolicyTests
{
    [Theory]
    [InlineData(15, 00, 00,14, 00, 00)]
    [InlineData(15, 00, 00, 15, 00, 00)]
    public void Validate_GivenTimeToNotAfterThanTimeFrom_ShouldThrowInvalidMeetingTimeSpanException(int timeFromHours, 
        int timeFromMinutes, int timeFromSeconds, int timeToHours, int timeToMinutes, int timeToSeconds)
    {
        //arrange
        var timeFrom = new TimeOnly(timeFromHours, timeFromMinutes, timeFromSeconds);
        var timeTo = new TimeOnly(timeToHours, timeToMinutes, timeToSeconds);
        var policy = MeetingTimePolicy.GetInstance(timeFrom, timeTo);
        
        //act
        var result = Record.Exception(() => policy.Validate());
        
        //assert
        result.ShouldBeOfType<InvalidMeetingTimeSpanException>();
    }
    
    [Theory]
    [InlineData(14, 00, 00,14, 00, 01)]
    [InlineData(20, 00, 00, 21,00, 00)]
    public void Validate_GivenValidTimes_ShouldNotThrow(int timeFromHours, 
        int timeFromMinutes, int timeFromSeconds, int timeToHours, int timeToMinutes, int timeToSeconds)
    {
        //arrange
        var timeFrom = new TimeOnly(timeFromHours, timeFromMinutes, timeFromSeconds);
        var timeTo = new TimeOnly(timeToHours, timeToMinutes, timeToSeconds);
        var policy = MeetingTimePolicy.GetInstance(timeFrom, timeTo);
        
        //act
        var result = Record.Exception(() => policy.Validate());
        
        //assert
        result.ShouldBeNull();
    }
}