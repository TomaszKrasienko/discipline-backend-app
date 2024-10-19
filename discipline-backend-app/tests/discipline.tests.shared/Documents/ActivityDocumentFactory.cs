using Bogus;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.tests.shared.Documents;

internal static class ActivityDocumentFactory
{
    internal static ActivityDocument Get()
        => Get(1).Single();
    
    internal static List<ActivityDocument> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<ActivityDocument> GetFaker()
        => new Faker<ActivityDocument>()
            .RuleFor(f => f.Id, v => Ulid.NewUlid())
            .RuleFor(f => f.Title, v => v.Random.String(length: 10, minChar: 'A', maxChar: 'z'))
            .RuleFor(f => f.IsChecked, v => false)
            .RuleFor(f => f.ParentRuleId, v => null);
}