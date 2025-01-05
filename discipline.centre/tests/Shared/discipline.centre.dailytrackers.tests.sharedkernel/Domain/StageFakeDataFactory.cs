using Bogus;
using discipline.centre.dailytrackers.domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.tests.sharedkernel.Domain;

public static class StageFakeDataFactory
{
    public static Stage Get(int index = 1)
    {
        var faker = new Faker<Stage>()
            .CustomInstantiator(x => Stage.Create(StageId.New(), x.Random.String(minLength: 3, maxLength: 30), index));
        return faker.Generate(1).Single();
    } 
}