using Bogus;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.sharedkernel.Infrastructure;

internal static class StageDocumentFakeDataFactory
{
    internal static StageDocument Get(int index = 1)
    {
        var faker = new Faker<StageDocument>()
            .RuleFor(x => x.StageId, StageId.New().ToString())
            .RuleFor(x => x.Title, f => f.Random.String(minLength: 3, maxLength: 30))
            .RuleFor(x => x.Index, index)
            .RuleFor(x => x.IsChecked, f => f.Random.Bool());

        return faker.Generate(1).Single();
    }
}