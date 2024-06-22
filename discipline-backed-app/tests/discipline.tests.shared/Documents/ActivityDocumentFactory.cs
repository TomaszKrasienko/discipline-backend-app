using Bogus;
using discipline.application.Infrastructure.DAL.Documents;

namespace discipline.tests.shared.Documents;

internal static class ActivityDocumentFactory
{
    private static ActivityDocument Get()
        => Get(1).Single();
    
    private static List<ActivityDocument> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<ActivityDocument> GetFaker()
        => new Faker<ActivityDocument>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Title, v => v.Random.String(length: 10, minChar: 'A', maxChar: 'z'))
            .RuleFor(f => f.IsChecked, v => false)
            .RuleFor(f => f.ParentRuleId, v => null);
}