using Bogus;
using discipline.application.Domain.Entities;
using discipline.application.Domain.ValueObjects.ActivityRules;

namespace discipline.tests.shared.Entities;

internal static class ActivityRuleFactory
{
    internal static ActivityRule Get(List<int> selectedDays = null)
        => Get(1, selectedDays).Single();
    
    internal static List<ActivityRule> Get(int count, List<int> selectedDays = null)
        => GetFaker(selectedDays).Generate(count);
    
    private static Faker<ActivityRule> GetFaker(List<int> selectedDays)
        => new Faker<ActivityRule>()
            .CustomInstantiator(arg => ActivityRule.Create(
                Guid.NewGuid(),
                arg.Random.String2(length: 10),
                selectedDays is null ? arg.PickRandom<string>(Mode.AvailableModes.Keys.Where(x => x != Mode.CustomMode()).ToList()) : Mode.CustomMode(),
                selectedDays));
}