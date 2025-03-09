using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;

namespace discipline.centre.calendar.domain.unit_tests.ValueObjects;

public sealed class DayTests
{
    [Fact]
    public void GivenCorrectValue_WhenCreate_ShouldReturnDayWithValue()
    {
        // Arrange
        var value = new DateOnly(2020, 1, 1);
        
        // Act
        var result = Day.Create(value);
        
        // Assert
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void GivenDefaultValue_WhenCreate_ShouldThrowDomainExceptionWithCodeCalendarEventDayDefault()
    {
        // Act
        var exception = Record.Exception(() => Day.Create(default));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("CalendarEvent.Day.Default");
    }
}