using Bogus;
using discipline.application.Domain.Entities;

namespace discipline.tests.shared.Entities;

internal static class DailyProductivityFactory
{
    internal static DailyProductivity Get(DateTime? now = null)
        => Get(1, now).Single();
    
    private static List<DailyProductivity> Get(int count, DateTime? now = null)
        => GetFaker(now).Generate(count);
    
    private static Faker<DailyProductivity> GetFaker(DateTime? now = null)
        => new Faker<DailyProductivity>()
            .CustomInstantiator(x => DailyProductivity.Create(
                now?.Date ?? DateTime.Now.Date));
}