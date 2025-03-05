using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.calendar.domain;

public sealed class ImportantDateEvent : BaseCalendarEvent
{
    private ImportantDateEvent(CalendarEventId id, DateOnly day, CalendarEventContent content) : base(id, day, content)
    {
        
    }
}