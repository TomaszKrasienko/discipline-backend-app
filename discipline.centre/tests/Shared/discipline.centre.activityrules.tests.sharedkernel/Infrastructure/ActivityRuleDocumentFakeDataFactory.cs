using Bogus;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.infrastructure.DAL.Documents;

namespace discipline.centre.activityrules.tests.sharedkernel.Infrastructure;

internal static class ActivityRuleDocumentFakeDataFactory
{
    internal static ActivityRuleDocument Get(List<int>? selectedDays = null)
        => Get(1, selectedDays).Single();
    
    private static List<ActivityRuleDocument> Get(int count, List<int>? selectedDays = null)
        => GetFaker(selectedDays).Generate(count);
    
    private static Faker<ActivityRuleDocument> GetFaker(List<int>? selectedDays = null)
        => new Faker<ActivityRuleDocument>()
            .RuleFor(f => f.Id, v => Ulid.NewUlid().ToString())
            .RuleFor(f => f.Title, v => v.Random.String(length: 10, minChar: 'A', maxChar: 'z'))
            .RuleFor(f => f.Note, v => v.Random.String(length: 10, minChar: 'A', maxChar: 'z'))
            .RuleFor(f => f.Mode,
                v => selectedDays is null
                    ? v.PickRandom<string>(Mode.AvailableModes.Keys.Where(x => x != Mode.CustomMode).ToList())
                    : Mode.CustomMode)
            .RuleFor(f => f.SelectedDays, v => selectedDays)
            .RuleFor(f => f.UserId, v => Ulid.NewUlid().ToString());
}