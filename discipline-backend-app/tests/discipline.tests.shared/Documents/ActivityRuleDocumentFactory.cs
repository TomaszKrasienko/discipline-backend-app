using Bogus;
using discipline.application.Infrastructure.DAL.Documents;

namespace discipline.tests.shared.Documents;

internal static class ActivityRuleDocumentFactory
{
    internal static ActivityRuleDocument Get(List<int> selectedDays = null)
        => Get(1, selectedDays).Single();
    
    internal static List<ActivityRuleDocument> Get(int count, List<int> selectedDays = null)
        => GetFaker(selectedDays).Generate(count);
    
    private static Faker<ActivityRuleDocument> GetFaker(List<int> selectedDays = null)
        => new Faker<ActivityRuleDocument>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Title, v => v.Random.String(length: 10, minChar: 'A', maxChar: 'z'))
            .RuleFor(f => f.Mode,
                v => selectedDays is null
                    ? v.PickRandom<string>(Mode.AvailableModes.Keys.Where(x => x != Mode.CustomMode()).ToList())
                    : Mode.CustomMode())
            .RuleFor(f => f.SelectedDays, v => selectedDays);
}