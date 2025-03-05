using Bogus;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.tests.sharedkernel.Domain;

public static class DailyTrackerFakeDataFactory
{
    public static DailyTracker Get(Activity? activity = null, UserId? userId = null)    
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
                userId ?? UserId.New(), 
                activity is null ? ActivityId.New() : activity.Id,
                new ActivityDetailsSpecification(
                    activity is null ? x.Random.String(minLength:3, maxLength:30, minChar: 'a', maxChar:'z' ) : activity.Details.Title,
                    activity?.Details.Note),
                activity?.ParentActivityRuleId,
                stageSpecifications
            ));
        
        return faker.Generate(1).Single();
    }
}