using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.centre.calendar.domain.unit_tests.UserCalendarDayTests;

public sealed class AddImportantDateTests
{
    [Fact]
    public void GivenNotExistingImportantDate_WhenAddImportantDate_ShouldAddEventToUserCalendarDay()
    {
        // Arrange
        var userCalendarDay = UserCalendarDay.CreateWithImportantDate(UserCalendarId.New(), UserId.New(),
            new DateOnly(2025, 2, 2), CalendarEventId.New(), "test", null);

        const string title = "new_event";
        
        // Act
        userCalendarDay.AddImportantDate(CalendarEventId.New(), title, null);
        
        // Assert
        userCalendarDay.Events.Any(x => x is ImportantDateEvent && x.Content.Title == title)
            .ShouldBeTrue();
    }

    [Fact]
    public void GivenAlreadyRegisteredTitleInImportantDate_WhenAddImportantDate_ShouldThrowDomainExceptionWithCodeUserCalendarDayImportantDateEventAlreadyRegistered()
    {
        // Arrange
        const string title = "title_event";
        
        var userCalendarDay = UserCalendarDay.CreateWithImportantDate(UserCalendarId.New(), UserId.New(),
            new DateOnly(2025, 2, 2), CalendarEventId.New(), title, null);

        // Act
        var exception = Record.Exception(() => userCalendarDay.AddImportantDate(CalendarEventId.New(), title, null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("UserCalendarDay.ImportantDateEvent.AlreadyRegistered");
    }
}