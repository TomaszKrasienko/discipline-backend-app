using System.Xml.XPath;
using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.centre.calendar.domain.unit_tests.TimeEventTests;

public sealed class CreateTests
{
    [Fact]
    public void GivenFromIsBiggerThanTo_WhenCreate_ThenShouldThrowDomainException()
    {
        // Arrange
        var to = new TimeOnly(11, 00);
        
        // Act
        var exception = Record.Exception(() => TimeEvent.Create(CalendarEventId.New(),
            to.AddHours(1), to, "test", null));

        // Assert
        exception.ShouldBeOfType<DomainException>();
    }

    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenShouldThrowDomainException()
    {
        // Act
        var exception = Record.Exception(() => TimeEvent.Create(CalendarEventId.New(), new TimeOnly(10, 00),
            new TimeOnly(12, 00), string.Empty, null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
    }

    [Fact]
    public void GivenCorrectValues_WhenCreate_ShouldREturnTimeEventWithValues()
    {
        // Arrange
        var from = new TimeOnly(10, 00);
        var to = new TimeOnly(12, 00);
        var id = CalendarEventId.New();
        const string title = "Title";
        const string description = "Description";
        
        // Act
        var result = TimeEvent.Create(id, from, to, title, description);
        
        // Assert
        result.TimeSpan.From.ShouldBe(from);
        result.TimeSpan.To.ShouldBe(to);
        result.Id.ShouldBe(id);
        result.Content.Title.ShouldBe(title);
        result.Content.Description.ShouldBe(description);
    }
}