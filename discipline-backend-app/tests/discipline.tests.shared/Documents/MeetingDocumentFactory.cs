using Bogus;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;

namespace discipline.tests.shared.Documents;

internal static class MeetingDocumentFactory
{
    internal static MeetingDocument Get(bool withTimeTo, bool online = true)
        => Get(1, withTimeTo, online).Single();
    
    private static List<MeetingDocument> Get(int count, bool withTimeTo, bool online = true)
        => GetFaker(withTimeTo, online).Generate(count);

    private static Faker<MeetingDocument> GetFaker(bool withTimeTo, bool online = true)
        => new Faker<MeetingDocument>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Title, v => v.Random.String(10, 'A', 'z'))
            .RuleFor(f => f.TimeFrom, v => new TimeOnly(v.Random.Int(5, 10), 0, 0))
            .RuleFor(f => f.TimeTo, v => withTimeTo ? new TimeOnly(v.Random.Int(10, 22), 1, 0) : null)
            .RuleFor(f => f.Platform, v => online ? v.Random.String(20, 'A', 'z') : null)
            .RuleFor(f => f.Uri, v => online ? v.Random.String(20, 'A', 'z') : null)
            .RuleFor(f => f.Place, v => !online ? v.Random.String(20, 'A', 'z') : null);
}