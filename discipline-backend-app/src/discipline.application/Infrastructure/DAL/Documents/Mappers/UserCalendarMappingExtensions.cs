using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Domain.UsersCalendars.ValueObjects.Event;
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

    internal static UserCalendar AsEntity(this UserCalendarDocument document)
        => new UserCalendar(document.Day, document.Events?.Select(x => x.AsEntity()).ToList());

    private static Event AsEntity(this EventDocument document) => document switch
    {
        CalendarEventDocument calendarEventDocument => calendarEventDocument.AsEntity(),
        ImportantDateDocument importantDateDocument => importantDateDocument.AsEntity(),
        MeetingDocument meetingDocument => meetingDocument.AsEntity()
    };

    private static ImportantDate AsEntity(this ImportantDateDocument document)
        => new ImportantDate(
            document.Id,
            document.Title);

    private static CalendarEvent AsEntity(this CalendarEventDocument document)
        => new CalendarEvent(
            document.Id,
            document.Title,
            new MeetingTimeSpan(document.TimeFrom,document.TimeTo),
            document.Action);

    private static Meeting AsEntity(this MeetingDocument document)
        => new Meeting(
            document.Id,
            document.Title,
            new MeetingTimeSpan(document.TimeFrom, document.TimeTo),
            new Address(document.Platform, document.Uri, document.Place));
}