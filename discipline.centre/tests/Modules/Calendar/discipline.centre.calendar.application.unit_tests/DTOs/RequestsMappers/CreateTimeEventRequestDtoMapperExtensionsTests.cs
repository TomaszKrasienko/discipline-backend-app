using discipline.centre.calendar.application.UserCalendar.TimeEvents.DTOs.Requests;
using discipline.centre.calendar.tests.sharedkernel.Application.Requests;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.centre.calendar.application.unit_tests.DTOs.RequestsMappers;

public sealed class CreateTimeEventRequestDtoMapperExtensionsTests
{
    [Fact]
    public void GivenCreateTimeEventRequestDtoWithUserIdDayAndEventId_WhenAsCommand_ThenMapToEntity()
    {
        // Arrange
        var dto = CreateTimeEventRequestDtoFakeDataFactory.Get(true, true);
        var userId = UserId.New();
        var day = DateOnly.FromDateTime(DateTime.UtcNow);
        var eventId = CalendarEventId.New();
        
        // Act
        var command = dto.AsCommand(userId, day, eventId);
        
        // Assert
        command.UserId.ShouldBe(userId);
        command.Day.ShouldBe(day);
        command.EventId.ShouldBe(eventId);
        command.Title.ShouldBe(dto.Title);
        command.Description.ShouldBe(dto.Description);
        command.TimeFrom.ShouldBe(dto.TimeFrom);
        command.TimeTo.ShouldBe(dto.TimeTo);
    }
}