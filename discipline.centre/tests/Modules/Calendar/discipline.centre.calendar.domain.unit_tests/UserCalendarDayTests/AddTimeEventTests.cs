using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.centre.calendar.domain.unit_tests.UserCalendarDayTests;

public sealed class AddTimeEventTests
{
    [Fact]
    public void GivenNotExistingTimeEvent_WhenAddTimeEvent_ShouldAddTimeEvent()
    {
        // Arrange
        var userCalendarDay = UserCalendarDay.CreateWithImportantDate(UserCalendarId.New(), UserId.New(),
            new DateOnly(2025, 2, 2), CalendarEventId.New(), "test", null);

        const string title = "new_event";
        
        // Act
        userCalendarDay.AddTimeEvent(CalendarEventId.New(), title, null, 
            new TimeOnly(20,00), null);
        
        // Assert
        userCalendarDay.Events.Any(x => x is ImportantDateEvent && x.Content.Title == title)
            .ShouldBeTrue();
    }

    [Fact]
    public void GivenExistingTimeEvent_WhenAddTimeEvent_ShouldThrowDomainExceptionWithCodeUserCalendarDayTimeEventAlreadyRegistered()
    {
        // Arrange
        const string title = "title_event";
        
        var userCalendarDay = UserCalendarDay.CreateWithTimeEvent(UserCalendarId.New(), UserId.New(),
            new DateOnly(2025, 2, 2), CalendarEventId.New(), title, null,
            new TimeOnly(20, 00), null);

        // Act
        var exception = Record.Exception(() => userCalendarDay.AddTimeEvent(CalendarEventId.New(), title, null,
            new TimeOnly(21,00), new TimeOnly(22,00)));

        // Assert
        exception.ShouldBeOfType<DomainException>();
    }
}