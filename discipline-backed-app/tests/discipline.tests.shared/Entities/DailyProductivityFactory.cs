using Bogus;
using discipline.application.Domain.Entities;

namespace discipline.tests.shared.Entities;

internal static class DailyProductivityFactory
{
    internal static DailyProductivity Get(DateOnly? now = null)
        => Get(1, now).Single();
    
    private static List<DailyProductivity> Get(int count, DateOnly? now = null)
        => GetFaker(now).Generate(count);
    
    private static Faker<DailyProductivity> GetFaker(DateOnly? now = null)
        => new Faker<DailyProductivity>()
            .CustomInstantiator(x => DailyProductivity.Create(
                now ?? DateOnly.FromDateTime(DateTime.Now.Date)));
}