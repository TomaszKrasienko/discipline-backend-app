using Bogus;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.sharedkernel.Domain;

public static class DailyTrackerFakeDataFactory
{
    public static DailyTracker Get(Activity? activity = null)
    {
        List<StageSpecification>? stageSpecifications = null;
        if (activity?.Stages != null)
        {
            stageSpecifications = [];
            stageSpecifications.AddRange(activity.Stages.Select(stage => new StageSpecification(stage.Title, stage.Index)));
        }
        
        var faker = new Faker<DailyTracker>()
            .CustomInstantiator(x => DailyTracker.Create(
                DailyTrackerId.New(), 
                DateOnly.FromDateTime(DateTime.UtcNow),
                UserId.New(), 
                ActivityId.New(),
                new ActivityDetailsSpecification(
                    activity is null ? x.Random.String(minLength:3, maxLength:30) : activity.Details.Title,
                    activity?.Details.Note),
                activity?.ParentActivityRuleId,
                stageSpecifications
            ));
        
        return faker.Generate(1).Single();
    }
}