namespace discipline.centre.calendar.application.UserCalendar.TimeEvents.DTOs.Requests;

public sealed record CreateTimeEventRequestDto(string Title, string? Description,
    TimeOnly TimeFrom, TimeOnly? TimeTo);