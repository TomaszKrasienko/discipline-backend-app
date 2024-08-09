using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.ValueObjects.UserCalendar;

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
        => new((Day)document.Day, (List<Event>)document.Events?.Select(x => x.AsEntity()).ToList());

    private static Event AsEntity(this EventDocument document) => document switch
    {
        CalendarEventDocument calendarEventDocument => calendarEventDocument.AsEntity(),
        ImportantDateDocument importantDateDocument => importantDateDocument.AsEntity(),
        MeetingDocument meetingDocument => meetingDocument.AsEntity()
    };

    private static ImportantDate AsEntity(this ImportantDateDocument document)
        => new(
            document.Id,
            document.Title);

    private static CalendarEvent AsEntity(this CalendarEventDocument document)
        => new(
            document.Id,
            document.Title,
            new MeetingTimeSpan(document.TimeFrom, document.TimeTo),
            document.Action);

    private static Meeting AsEntity(this MeetingDocument document)
        => new(
            document.Id,
            document.Title,
            new MeetingTimeSpan(document.TimeFrom, document.TimeTo),
            new Address(document.Platform, document.Uri, document.Place));

    internal static UserCalendarDto AsDto(this UserCalendarDocument document)
        => new()
        {
            Day = document.Day,
            ImportantDates = document.Events
                .Where(x => x.GetType() == typeof(ImportantDateDocument))
                .Select(x => ((ImportantDateDocument)x).AsDto())
                .ToList(),
            CalendarEvents = document.Events
                .Where(x => x.GetType() == typeof(CalendarEventDocument))
                .Select(x => ((CalendarEventDocument)x).AsDto())
                .ToList(),
            Meetings = document.Events
                .Where(x => x.GetType() == typeof(MeetingDocument))
                .Select(x => ((MeetingDocument)x).AsDto())
                .ToList()
        };

private static ImportantDateDto AsDto(this ImportantDateDocument document)
        => new()
        {
            Id = document.Id,
            Title = document.Title
        };

    private static CalendarEventDto AsDto(this CalendarEventDocument document)
        => new()
        {
            Id = document.Id,
            Title = document.Title,
            TimeFrom = document.TimeFrom,
            TimeTo = document.TimeTo,
            Action = document.Action
        };

    private static MeetingDto AsDto(this MeetingDocument document)
        => new ()
        {
            Id = document.Id,
            Title = document.Title,
            TimeFrom = document.TimeFrom,
            TimeTo = document.TimeTo,
            Platform = document.Platform,
            Uri = document.Uri,
            Place = document.Place
        };
}