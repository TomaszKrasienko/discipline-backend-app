using Bogus;
using discipline.infrastructure.DAL.Documents.UsersCalendar;

namespace discipline.tests.shared.Documents;

internal static class CalendarEventDocumentFactory
{
    internal static CalendarEventDocument Get(bool withTimeTo)
        => Get(1, withTimeTo).Single();
    
    private static List<CalendarEventDocument> Get(int count, bool withTimeTo)
        => GetFaker(withTimeTo).Generate(count);

    private static Faker<CalendarEventDocument> GetFaker(bool withTimeTo)
        => new Faker<CalendarEventDocument>()
            .RuleFor(f => f.Id, v => Ulid.NewUlid().ToString())
            .RuleFor(f => f.Title, v => v.Random.String(10, 'A', 'z'))
            .RuleFor(f => f.TimeFrom, v => new TimeOnly(v.Random.Int(5, 10), 0, 0))
            .RuleFor(f => f.TimeTo, v => withTimeTo ? new TimeOnly(v.Random.Int(10, 22), 1, 0) : null)
            .RuleFor(f => f.Action, v => v.Random.String(20, 'A', 'z'));
}