using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.centre.calendar.domain.unit_tests.ImportantDateEventTests;

public sealed class CreateTests
{
    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenShouldThrowDomainException()
    {
        // Act
        var exception = Record.Exception(() => ImportantDateEvent.Create(CalendarEventId.New(),
            string.Empty, null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
    }

    [Fact]
    public void GivenCorrectValues_WhenCreate_ShouldReturnImportantDateEventWithValues()
    {
        // Arrange
        var id =  CalendarEventId.New();
        const string title = "title";
        const string description = "description";
        
        // Act
        var result = ImportantDateEvent.Create(id, title, description);
        
        // Assert
        result.Id.ShouldBe(id);
        result.Content.Title.ShouldBe(title);
        result.Content.Description.ShouldBe(description);
    }
}