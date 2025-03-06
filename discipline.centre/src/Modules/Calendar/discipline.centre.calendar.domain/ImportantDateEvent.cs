using discipline.centre.calendar.domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discpline.centre.calendar.domain;

public sealed class ImportantDateEvent : BaseCalendarEvent
{
    private ImportantDateEvent(CalendarEventId id, DateOnly day, CalendarEventContent content) : base(id, day, content)
    {
        
    }
}