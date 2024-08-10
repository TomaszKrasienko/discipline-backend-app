using Bogus;
using discipline.domain.UsersCalendars.Entities;

namespace discipline.tests.shared.Entities;

internal static class ImportantDateFactory
{
    internal static ImportantDate GetInUserCalender(UserCalendar userCalendar)
    {
        var importantDate = Get();
        userCalendar.AddEvent(importantDate.Id, importantDate.Title);
        return importantDate;
    }
    
    private static ImportantDate Get()
        => Get(1).Single();
    
    private static List<ImportantDate> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<ImportantDate> GetFaker()
        => new Faker<ImportantDate>()
            .CustomInstantiator(v => ImportantDate.Create(
                Guid.NewGuid(), v.Random.String(10, 'A', 'z')));
}