using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;

namespace discipline.centre.calendar.domain.unit_tests.ValueObjects;

public sealed class CalendarEventContentTests
{
    [Fact]
    public void GivenEmptyTitle_WhenCreate_ShouldThrowDomainExceptionWithCodeCalendarEventContentTitleEmpty()
    {
        // Act
        var exception = Record.Exception(() => CalendarEventContent.Create(string.Empty, null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("CalendarEvent.Content.Title.Empty");
    }

    [Fact]
    public void GivenCorrectValues_WhenCreate_ShouldReturnCalendarEventContentWithValues()
    {
        // Arrange
        const string title = "Title";
        const string description = "Description";
        
        // Act
        var result = CalendarEventContent.Create(title, description);
        
        // Assert
        result.Title.ShouldBe(title);
        result.Description.ShouldBe(description);
    }
}