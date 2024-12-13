using Bogus;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.sharedkernel.Domain;

public static class ActivityFakeDataFactory
{
    public static Activity Get(bool withNote = false, bool withParent = false, List<Stage>? stages = null)
    {
        List<StageSpecification> stageSpecifications = null;
        if (stages is not null)
        {
            stageSpecifications = [];
            foreach (var stage in stages)
            {
                stageSpecifications.Add(new StageSpecification(stage.Title, stage.Index));
            }
        }

        var faker = new Faker<Activity>()
            .CustomInstantiator(x => Activity.Create(
                ActivityId.New(),
                new ActivityDetailsSpecification(x.Random.String(minLength: 3, maxLength: 30), 
                    withNote ? x.Lorem.Word() : null),
                withParent ? ActivityRuleId.New() : null,
                stageSpecifications));

        return faker.Generate(1).Single();
    }
}