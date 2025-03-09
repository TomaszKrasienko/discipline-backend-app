using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.calendar.domain;

public sealed class UserCalendarDay : AggregateRoot<UserCalendarId, Ulid>
{
    private readonly HashSet<BaseCalendarEvent> _events = new();
    public UserId UserId { get; }
    public Day Day { get; }
    public IReadOnlySet<BaseCalendarEvent> Events => _events;

    private UserCalendarDay(UserCalendarId id, UserId userId, Day day) : base(id)
    {
        UserId = userId;
        Day = day;
    }

    public static UserCalendarDay CreateWithImportantDate(UserCalendarId id,
        UserId userId, 
        DateOnly day,
        CalendarEventId eventId,
        string title, 
        string? description)
    {
        var userCalendarDay = new UserCalendarDay(id, userId, day);
        userCalendarDay._events.Add(ImportantDateEvent.Create(eventId, title, description));
        return userCalendarDay;
    }

    public static UserCalendarDay CreateWithTimeEvent(UserCalendarId id,
        UserId userId,
        Day day,
        CalendarEventId eventId,
        string title,
        string? description,
        TimeOnly timeFrom,
        TimeOnly? timeTo)
    {
        var userCalendarDay = new UserCalendarDay(id, userId, day);
        userCalendarDay._events.Add(TimeEvent.Create(eventId, timeFrom, timeTo, title, description));
        return userCalendarDay;
    }
}