using discipline.centre.calendar.domain;
using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.calendar.infrastructure.DAL.Documents;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.calendar.infrastructure.DAL.Calendar.Documents;

internal static class UserCalendarDayDocumentMapperExtensions
{
    internal static UserCalendarDay AsEntity(this UserCalendarDayDocument document)
        => new (
            UserCalendarId.Parse(document.UserCalendarId), 
            UserId.Parse(document.UserId),
            document.Day,
            document.Events.Select(x => x.AsEntity()).ToHashSet());

    private static BaseCalendarEvent AsEntity(this BaseCalendarEventDocument document) => document switch
    {
        ImportantDateEventDocument importantDateDocument => importantDateDocument.AsEntity(),
        TimeEventDocument timeEventDocument => timeEventDocument.AsEntity(),
        _ => throw new ArgumentOutOfRangeException(nameof(document))
    };
    
    private static ImportantDateEvent AsEntity(this ImportantDateEventDocument document)
        => new (CalendarEventId.Parse(document.EventId), 
            CalendarEventContent.Create(document.Content.Title, document.Content.Description));

    private static TimeEvent AsEntity(this TimeEventDocument document)
        => new(CalendarEventId.Parse(document.EventId),
            EventTimeSpan.Create(document.TimeSpan.From, document.TimeSpan.To),
            CalendarEventContent.Create(document.Content.Title, document.Content.Description));
}