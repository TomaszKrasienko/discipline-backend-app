using Bogus;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;

namespace discipline.tests.shared.Documents;

internal static class ImportantDateDocumentFactory
{
    internal static ImportantDateDocument Get()
        => Get(1).Single();
    
    private static List<ImportantDateDocument> Get(int count)
        => GetFaker().Generate(count);

    private static Faker<ImportantDateDocument> GetFaker()
        => new Faker<ImportantDateDocument>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Title, v => v.Random.String(10, 'A', 'z'));
}