using Bogus;
using discipline.domain.UsersCalendars.Entities;

namespace discipline.tests.shared.Entities;

internal static class UserCalendarFactory
{
    internal static UserCalendar Get()
        => Get(1).Single();
    
    private static List<UserCalendar> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<UserCalendar> GetFaker()
        => new Faker<UserCalendar>().CustomInstantiator(x =>
            UserCalendar.Create(DateOnly.FromDateTime(DateTime.Now)));
}