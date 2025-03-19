using discipline.centre.calendar.application.UserCalendar.TimeEvents.Commands.CreateTimeEvent;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.calendar.application.UserCalendar.TimeEvents.DTOs.Requests;

public static class CreateTimeEventRequestDtoMapperExtensions
{
    public static CreateTimeEventCommand AsCommand(this CreateTimeEventRequestDto dto, 
        UserId userId, 
        DateOnly day,
        CalendarEventId eventId)
        => new(userId, day, eventId, dto.Title, dto.Description, dto.TimeFrom, dto.TimeTo);
}