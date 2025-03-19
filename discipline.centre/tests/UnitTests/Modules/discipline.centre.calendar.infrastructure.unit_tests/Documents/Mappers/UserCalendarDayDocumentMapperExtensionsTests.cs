using discipline.centre.calendar.domain;
using discipline.centre.calendar.infrastructure.DAL.Calendar.Documents;
using discipline.centre.calendar.infrastructure.DAL.Documents;
using discipline.centre.calendar.tests.sharedkernel.Infrastructure;
using Shouldly;

namespace discipline.centre.calendar.infrastructure.unit_tests.Documents.Mappers;

public sealed class UserCalendarDayDocumentMapperExtensionsTests
{
    [Fact]
    public void GivenUserCalendarDayDocumentMapper_WhenAsEntity_ThenShouldReturnUserCalendarDay()
    {
        // Arrange
        var document = UserCalendarDayDocumentFakeDataFactory
            .Get()
            .AddTimeEventDocument()
            .AddImportantDayDocument();
        
        var timeEventDocument = (TimeEventDocument)document.Events.Single(x => x is TimeEventDocument);
        var importantDateEventDocument = (ImportantDateEventDocument)document.Events.Single(x => x is ImportantDateEventDocument);
        
        // Act
        var result = document.AsEntity();
        
        // Assert
        result.Id.Value.ToString().ShouldBe(document.UserCalendarId);
        result.UserId.Value.ToString().ShouldBe(document.UserId);
        result.Day.Value.ShouldBe(document.Day);

        var timeEvent = (TimeEvent)result.Events.Single(x => x is TimeEvent);
        timeEvent.Id.Value.ToString().ShouldBe(timeEventDocument.EventId);
        timeEvent.Content.Title.ShouldBe(timeEventDocument.Content.Title);
        timeEvent.Content.Description.ShouldBe(timeEventDocument.Content.Description);
        timeEvent.TimeSpan.From.ShouldBe(timeEventDocument.TimeSpan.From);
        timeEvent.TimeSpan.To.ShouldBe(timeEventDocument.TimeSpan.To);
        
        var importantDateEvent = (ImportantDateEvent)result.Events.Single(x => x is ImportantDateEvent);
        importantDateEvent.Id.Value.ToString().ShouldBe(importantDateEventDocument.EventId);
        importantDateEvent.Content.Title.ShouldBe(importantDateEventDocument.Content.Title);
        importantDateEvent.Content.Description.ShouldBe(importantDateEventDocument.Content.Description);
    }
}