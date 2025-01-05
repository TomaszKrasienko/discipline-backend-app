using Bogus;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.tests.sharedkernel.Infrastructure;

internal static class ActivityDocumentFakeDataFactory
{
    internal static ActivityDocument Get(bool withNote = false, bool withParentActivity = false, List<StageDocument>? stages = null)
    {
        var faker = new Faker<ActivityDocument>()
            .RuleFor(x => x.ActivityId, ActivityId.New().ToString())
            .RuleFor(x => x.Title, f => f.Random.String(minLength: 3, maxLength: 30))
            .RuleFor(x => x.Note, f => withNote ? f.Lorem.Sentence() : null)
            .RuleFor(x => x.ParentActivityRuleId, withParentActivity ? ActivityRuleId.New().ToString() :  null)
            .RuleFor(x => x.Stages, stages);
        
        return faker.Generate(1).Single();
    }
}