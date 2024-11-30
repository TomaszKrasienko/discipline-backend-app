using Bogus;
using discipline.centre.users.application.Users.DTOs.Endpoints;

namespace discipline.centre.users.tests.sharedkernel.Application;

public static class SignUpRequestFakeDataFactory
{
    public static SignUpRequest Get()
    {
        var faker = new Faker<SignUpRequest>()
            .CustomInstantiator(v => new SignUpRequest(
                v.Internet.Email(),
                "Test123!",
                v.Name.FirstName(),
                v.Name.LastName()));
        return faker.Generate(1).Single();
    }
}