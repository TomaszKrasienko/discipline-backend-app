using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.centre.calendar.domain.unit_tests.UserCalendarDayTests;

public class CreateWithTimeEventTests
{
    [Fact]
    public void GivenValidArguments_WhenCreateWithTimeEvent_ThenShouldReturnUserCalendarDayWithTimeEvent()
    {
        // Arrange
        var userCalendarId = UserCalendarId.New();
        var userId = UserId.New();
        var day = new DateOnly(2025, 1, 1);
        var calendarEventId = CalendarEventId.New();
        const string title = "Title";
        const string description = "Description";
        var from = new TimeOnly(10, 00);
        var to = new TimeOnly(20, 00);
        
        // Act
        var result = UserCalendarDay.CreateWithTimeEvent(userCalendarId, userId, day, calendarEventId, 
            title, description, from, to);
        
        // Assert
        result.Id.ShouldBe(userCalendarId);
        result.UserId.ShouldBe(userId);
        result.Day.Value.ShouldBe(day);
        result.Events.Any(x 
            => x is TimeEvent @event
               && @event.Id == calendarEventId
               && @event.Content is { Title: title, Description: description }
               && @event.TimeSpan.From == from 
               && @event.TimeSpan.To == to).ShouldBeTrue();
    }

    [Fact]
    public void GivenDefaultDayDateOnly_WhenCreateWithTimeEvent_ThenShouldThrowDomainExceptionWithCodeDomainExceptionWithCodeCalendarEventDayDefault()
    {
        // Act
        var exception = Record.Exception(() => UserCalendarDay.CreateWithTimeEvent(UserCalendarId.New(), UserId.New(), default, CalendarEventId.New(), 
            "Title", "Description", new TimeOnly(10, 00), new TimeOnly(20, 00)));
        
        // Arrange
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("CalendarEvent.Day.Default");
    }
}