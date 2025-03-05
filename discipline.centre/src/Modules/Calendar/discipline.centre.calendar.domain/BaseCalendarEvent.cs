using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.calendar.domain;

public abstract class BaseCalendarEvent : Entity<CalendarEventId, Ulid>
{
    public Day Day { get; }
    public CalendarEventContent Content { get; }

    protected BaseCalendarEvent(CalendarEventId id, DateOnly day, CalendarEventContent content) : base(id)
    {
        Day = day;
        Content = content;
    }
}