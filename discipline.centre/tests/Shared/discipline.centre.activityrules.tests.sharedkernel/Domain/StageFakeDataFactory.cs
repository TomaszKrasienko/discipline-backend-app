using Bogus;
using discipline.centre.activityrules.domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.tests.sharedkernel.Domain;

internal static class StageFakeDataFactory
{
    internal static Stage Get(int index)
        => Get(index, 1).Single();
    
    private static List<Stage> Get(int count, int index)
        => GetFaker(index).Generate(count);
    
    private static Faker<Stage> GetFaker(int index)
        => new Faker<Stage>().CustomInstantiator(f => Stage.Create(StageId.New(),
            f.Random.String(minLength: 3, maxLength: 30), index));

}