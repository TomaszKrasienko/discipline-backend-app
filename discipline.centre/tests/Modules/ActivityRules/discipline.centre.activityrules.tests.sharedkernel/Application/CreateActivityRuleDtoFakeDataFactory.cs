using Bogus;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain.ValueObjects;

namespace discipline.centre.activityrules.tests.sharedkernel.Application;

public static class CreateActivityRuleDtoFakeDataFactory
{
    public static CreateActivityRuleDto Get()
    {
        var mode = new Faker()
            .PickRandom<string>(Mode.AvailableModes.Keys);

        var random = new Random();
        var selectedDaysCount = random.Next(1, 6);
        List<int> days = [];
        for (int i = 0; i < selectedDaysCount; i++)
        {
            days.Add(i);
        }

        var faker = new Faker<CreateActivityRuleDto>()
            .CustomInstantiator(v => new CreateActivityRuleDto(
                v.Lorem.Word(),
                mode,
                mode == Mode.CustomMode ? days : null));

        return faker.Generate(1).Single();
    }
}