using Bogus;
using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.domain.Specifications;

namespace discipline.centre.dailytrackers.sharedkernel.Application;

public static class CreateActivityDtoFakeDataFactory
{
    public static CreateActivityDto Get(bool withStages = false)
    {
        var faker = new Faker<CreateActivityDto>()
            .CustomInstantiator(v => new CreateActivityDto(
                v.Date.PastDateOnly(),
                new ActivityDetailsSpecification(v.Random.String(minLength: 10, maxLength: 20),
                    v.Random.String(minLength: 10, maxLength: 20)),
                withStages ? [new StageSpecification(v.Random.String(minLength: 10, maxLength: 20), 1)] : null));

        return faker.Generate(1).Single();
    }
}