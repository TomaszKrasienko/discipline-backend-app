using Bogus;
using discipline.infrastructure.DAL.Documents.DailyProductivities;

namespace discipline.tests.shared.Documents;

internal static class DailyProductivityDocumentFactory
{
    internal static DailyProductivityDocument Get(IEnumerable<ActivityDocument> activities = null)
        => Get(1, activities).Single();
    
    private static List<DailyProductivityDocument> Get(int count, IEnumerable<ActivityDocument> activities = null)
        => GetFaker(activities).Generate(count);
    
    private static Faker<DailyProductivityDocument> GetFaker(IEnumerable<ActivityDocument> activities = null)
        => new Faker<DailyProductivityDocument>()
            .RuleFor(f => f.Id, v => Ulid.NewUlid().ToString())
            .RuleFor(f => f.Day, v => DateOnly.FromDateTime(DateTime.Now.Date))
            .RuleFor(f => f.UserId, v => Ulid.NewUlid().ToString())
            .RuleFor(f => f.Activities, v => activities);
}