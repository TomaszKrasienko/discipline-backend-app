using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.centre.calendar.domain.unit_tests.UserCalendarDayTests;

public sealed class CreateWithImportantDateTests
{
    [Fact]
    public void GivenValidArguments_WhenCreateWithImportantDate_ThenShouldReturnUserCalendarDayWithImportantDate()
    {
        // Arrange
        var userCalendarId = UserCalendarId.New();
        var userId = UserId.New();
        var day = new DateOnly(2025, 1, 1);
        var calendarEventId = CalendarEventId.New();
        const string title = "Title";
        const string description = "Description";
        
        // Act
        var result = UserCalendarDay.CreateWithImportantDate(userCalendarId, userId, day, calendarEventId, 
            title, description);
        
        // Assert
        result.Id.ShouldBe(userCalendarId);
        result.UserId.ShouldBe(userId);
        result.Day.Value.ShouldBe(day);
        result.Events.Any(x 
            => x is ImportantDateEvent
               && x.Id == calendarEventId
               && x.Content is { Title: title, Description: description }).ShouldBeTrue();
    }
    
    [Fact]
    public void GivenDefaultDayDateOnlyValue_WhenCreateWithImportantDate_ThenShouldThrowDomainExceptionWithCodeCalendarEventDayDefault()
    {
        // Act
        var exception = Record.Exception(() => UserCalendarDay.CreateWithImportantDate(UserCalendarId.New(), UserId.New(), default, 
            CalendarEventId.New(), "Title", "Description"));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("CalendarEvent.Day.Default");
    }
}