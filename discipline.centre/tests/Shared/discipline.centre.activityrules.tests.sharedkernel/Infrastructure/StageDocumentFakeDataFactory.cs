using Bogus;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.tests.sharedkernel.Infrastructure;

internal static class StageDocumentFakeDataFactory
{
    public static StageDocument Get(int index = 1)
        => Get(index, 1).Single();

    private static List<StageDocument> Get(int index = 1, int count = 1)
        => GetFaker(index).Generate(count);

    private static Faker<StageDocument> GetFaker(int index = 1)
        => new Faker<StageDocument>()
            .RuleFor(f => f.StageId, v => StageId.New().ToString())
            .RuleFor(f => f.Title, v => v.Random.String(minLength: 3, maxLength: 30))
            .RuleFor(f => f.Index, v => index);
}