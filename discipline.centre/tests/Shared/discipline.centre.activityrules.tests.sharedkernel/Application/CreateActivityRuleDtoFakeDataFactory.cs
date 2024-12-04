using Bogus;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;

namespace discipline.centre.activityrules.tests.sharedkernel.Application;

public static class CreateActivityRuleDtoFakeDataFactory
{
    public static CreateActivityRuleDto Get()
    {
        //todo: add stages
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
                new ActivityRuleDetailsSpecification(v.Lorem.Word(), v.Lorem.Word()),
                mode,
                mode == Mode.CustomMode ? days : null,
                null));

        return faker.Generate(1).Single();
    }
}