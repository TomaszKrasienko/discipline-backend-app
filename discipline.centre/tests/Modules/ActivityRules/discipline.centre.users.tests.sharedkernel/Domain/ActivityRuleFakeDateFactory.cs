using Bogus;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.tests.sharedkernel.Domain;

public static class ActivityRuleFakeDateFactory
{
    public static ActivityRule Get(List<int>? selectedDays = null)
        => Get(1, selectedDays).Single();
    
    private static List<ActivityRule> Get(int count, List<int>? selectedDays = null)
        => GetFaker(selectedDays).Generate(count);
    
    private static Faker<ActivityRule> GetFaker(List<int>? selectedDays)
        => new Faker<ActivityRule>()
            .CustomInstantiator(arg => ActivityRule.Create(
                ActivityRuleId.New(), 
                UserId.New(), 
                arg.Random.String2(length: 10),
                selectedDays is null 
                    ? arg.PickRandom<string>(Mode.AvailableModes.Keys.Where(x => x != Mode.CustomMode).ToList()) 
                    : Mode.CustomMode,
                selectedDays));
}