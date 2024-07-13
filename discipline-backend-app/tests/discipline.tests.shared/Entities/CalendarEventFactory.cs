using Bogus;
using discipline.application.Domain.UsersCalendars.Entities;

namespace discipline.tests.shared.Entities;

internal static class CalendarEventFactory
{
    internal static CalendarEvent GetInUserCalender(UserCalendar userCalendar, bool withTimeTo = false)
    {
        var calendarEvent = Get(withTimeTo);
        userCalendar.AddEvent(calendarEvent.Id, calendarEvent.Title, calendarEvent.MeetingTimeSpan.From,
            calendarEvent.MeetingTimeSpan.To, calendarEvent.Action);
        return calendarEvent;
    }
    
    private static CalendarEvent Get(bool withTimeTo = false)
        => Get(1, withTimeTo).Single();
    
    private static List<CalendarEvent> Get(int count, bool withTimeTo = false)
        => GetFaker(withTimeTo).Generate(count);
    
    private static Faker<CalendarEvent> GetFaker(bool withTimeTo = false)
        => new Faker<CalendarEvent>()
            .CustomInstantiator(v => CalendarEvent.Create(
                Guid.NewGuid(),
                v.Random.String(10, 'A', 'z'),
                new TimeOnly(v.Random.Int(5, 10), 0, 0),
                withTimeTo ? new TimeOnly(v.Random.Int(10, 22), 1, 0) : null,
                v.Random.String(20, 'A', 'z')));
}