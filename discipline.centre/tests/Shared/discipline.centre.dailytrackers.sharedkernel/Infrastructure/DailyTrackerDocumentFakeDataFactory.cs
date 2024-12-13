using Bogus;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.sharedkernel.Infrastructure;

internal static class DailyTrackerDocumentFakeDataFactory
{
    internal static DailyTrackerDocument Get(List<ActivityDocument>? activities = null)
    {
        var faker = new Faker<DailyTrackerDocument>()
            .RuleFor(x => x.DailyTrackerId, DailyTrackerId.New().ToString())
            .RuleFor(x => x.Day, DateOnly.FromDateTime(DateTime.UtcNow))
            .RuleFor(x => x.UserId, UserId.New().ToString())
            .RuleFor(x => x.Activities, activities);
        
        return faker.Generate(1).Single();
    }
}