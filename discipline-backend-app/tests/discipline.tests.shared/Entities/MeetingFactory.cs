using Bogus;
using discipline.application.Domain.UsersCalendars.Entities;

namespace discipline.tests.shared.Entities;

internal static class MeetingFactory
{
    internal static Meeting GetInUserCalender(UserCalendar userCalendar, bool withTimeTo = false, 
        bool online = true)
    {
        var meeting = Get(withTimeTo, online);
        userCalendar.AddEvent(meeting.Id, meeting.Title, meeting.MeetingTimeSpan.From,
            meeting.MeetingTimeSpan.To, meeting.Address.Platform, meeting.Address.Uri, 
            meeting.Address.Place);
        return meeting;
    }
    
    private static Meeting Get(bool withTimeTo = false, bool online = true)
        => Get(1, withTimeTo, online).Single();
    
    private static List<Meeting> Get(int count,bool withTimeTo = false, bool online = true)
        => GetFaker(withTimeTo, online).Generate(count);
    
    private static Faker<Meeting> GetFaker(bool withTimeTo = false, bool online = true)
        => new Faker<Meeting>()
            .CustomInstantiator(v => Meeting.Create(
                Guid.NewGuid(),
                v.Random.String(10, 'A', 'z'),
                new TimeOnly(v.Random.Int(5, 10), 0, 0),
                withTimeTo ? new TimeOnly(v.Random.Int(10, 22), 1, 0) : null,
                online ? v.Random.String(20, 'A', 'z') : null,
                online ? v.Random.String(20, 'A', 'z') : null,
                !online ? v.Random.String(20, 'A', 'z') : null));
}