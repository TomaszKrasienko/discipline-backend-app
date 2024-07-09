using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.UsersCalendars.ValueObjects.UserCalendar;

namespace discipline.application.Domain.UsersCalendars.Entities;

internal sealed class UserCalendar : AggregateRoot
{
    private readonly List<Event> _events = [];
    public Day Day { get; }
    public IReadOnlyList<Event> Events => _events;

    private UserCalendar(Day day)
        => Day = day;

    internal static UserCalendar Create(DateOnly day)
        => new UserCalendar(day);

    internal void AddEvent(Guid id, string title)
        => _events.Add(ImportantDate.Create(id, title));
}