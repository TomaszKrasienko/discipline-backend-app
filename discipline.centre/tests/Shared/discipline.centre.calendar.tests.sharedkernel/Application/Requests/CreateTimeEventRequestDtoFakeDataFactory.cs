using Bogus;
using discipline.centre.calendar.application.UserCalendar.TimeEvents.DTOs.Requests;

namespace discipline.centre.calendar.tests.sharedkernel.Application.Requests;

public static class CreateTimeEventRequestDtoFakeDataFactory
{
    public static CreateTimeEventRequestDto Get(bool description = false, bool timeTo = false)
    {
        var faker = new Faker<CreateTimeEventRequestDto>()
            .CustomInstantiator(v => new CreateTimeEventRequestDto(
                v.Random.Word(),
                description ? v.Random.Word() : null,
                new TimeOnly(v.Random.Int(min: 1, max: 10), 0, 0),
                    null));
        
        var dto = faker.Generate(1).Single();
        return dto with { TimeTo = timeTo ? dto.TimeFrom.AddHours(1) : null };
    }
}