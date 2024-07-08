using Bogus;
using discipline.application.Domain.DailyProductivities.Entities;

namespace discipline.tests.shared.Entities;

internal static class ActivityFactory
{
    internal static Activity GetInDailyProductivity(DailyProductivity dailyProductivity)
    {
        var activity = Get();
        dailyProductivity.AddActivity(activity.Id, activity.Title);
        return activity;
    }
    
    internal static Activity Get()
        => Get(1).Single();
    
    internal static List<Activity> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<Activity> GetFaker()
        => new Faker<Activity>()
            .CustomInstantiator(x => Activity.Create(
                Guid.NewGuid(),
                x.Random.String(10, minChar: 'A', maxChar: 'z')));
}