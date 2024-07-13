using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;

namespace discipline.application.Infrastructure.DAL.Documents.Mappers;

internal static class UserCalendarMappingExtensions
{
    internal static UserCalendarDocument AsDocument(this UserCalendar entity)
        => new()
        {
            Day = entity.Day,
            Events = entity.Events?.Select(x => x.AsEventDocument())
        };

    private static EventDocument AsEventDocument(this Event @event) => @event switch
    {
        CalendarEvent calendarEvent => calendarEvent.AsDocument(),
        ImportantDate date => date.AsDocument(),
        Meeting meeting => meeting.AsDocument()
    };
    
    private static ImportantDateDocument AsDocument(this ImportantDate entity)
        => new()
        {
            Id = entity.Id,
            Title = entity.Title
        };

    private static CalendarEventDocument AsDocument(this CalendarEvent entity)
        => new()
        {
            Id = entity.Id,
            Title = entity.Title,
            Action = entity.Action,
            TimeFrom = entity.MeetingTimeSpan.From,
            TimeTo = entity.MeetingTimeSpan.To
        };

    private static MeetingDocument AsDocument(this Meeting entity)
        => new()
        {
            Id = entity.Id,
            Title = entity.Title,
            TimeFrom = entity.MeetingTimeSpan.From,
            TimeTo = entity.MeetingTimeSpan.To,
            Place = entity.Address.Place,
            Platform = entity.Address.Platform,
            Uri = entity.Address.Uri
        };
}