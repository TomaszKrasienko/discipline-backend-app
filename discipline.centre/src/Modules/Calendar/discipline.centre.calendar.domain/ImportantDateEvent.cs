using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.calendar.domain;

public sealed class ImportantDateEvent : BaseCalendarEvent
{
    /// <summary>
    /// Use only for mongo purpose!
    /// </summary>
    public ImportantDateEvent(CalendarEventId id, CalendarEventContent content) : base(id, content)
    { }

    public static ImportantDateEvent Create(CalendarEventId id, string title, string? description)
    {
        var content = CalendarEventContent.Create(title, description);
        return new ImportantDateEvent(id, content);
    }
}