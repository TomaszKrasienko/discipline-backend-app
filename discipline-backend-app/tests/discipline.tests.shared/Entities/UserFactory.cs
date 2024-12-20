using Bogus;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users;
using discipline.domain.Users.Entities;

namespace discipline.tests.shared.Entities;

internal static class UserFactory
{
    internal static User Get()
        => Get(1).Single();
    
    private static List<User> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<User> GetFaker()
        => new Faker<User>().CustomInstantiator(v =>
            User.Create(UserId.New(), v.Internet.Email(),
                v.Random.String(minChar:'a', maxChar:'z'), v.Name.FirstName(), v.Name.LastName()));
}