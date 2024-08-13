using discipline.domain.SharedKernel;
using discipline.domain.UsersCalendars.ValueObjects.UserCalendar;

namespace discipline.domain.UsersCalendars.Entities;

public sealed class UserCalendar : AggregateRoot
{
    private readonly List<Event> _events = [];
    public Day Day { get; }
    public EntityId UserId { get; }
    public IReadOnlyList<Event> Events => _events;

    private UserCalendar(Day day, EntityId userId)
    {
        Day = day;
        UserId = userId;
    } 
    
    //For mongo
    public UserCalendar(Day day, EntityId userId, List<Event> events) : this(day, userId)
        => _events = events;

    public static UserCalendar Create(DateOnly day, Guid userId)
        => new UserCalendar(day, userId);

    public void AddEvent(Guid id, string title)
        => _events.Add(ImportantDate.Create(id, title));

    public void AddEvent(Guid id, string title, TimeOnly timeFrom,
        TimeOnly? timeTo, string action)
        => _events.Add(CalendarEvent.Create(id, title, timeFrom, timeTo, action));

    public void AddEvent(Guid id, string title, TimeOnly timeFrom, TimeOnly? timeTo,
        string platform, string uri, string place)
        => _events.Add(Meeting.Create(id, title, timeFrom, timeTo, platform, uri, place));
}