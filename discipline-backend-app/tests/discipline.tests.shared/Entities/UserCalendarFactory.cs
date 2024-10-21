using Bogus;
using discipline.domain.SharedKernel.TypeIdentifiers;
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
            UserCalendar.Create(UserCalendarId.New(), DateOnly.FromDateTime(DateTime.Now), UserId.New()));
}