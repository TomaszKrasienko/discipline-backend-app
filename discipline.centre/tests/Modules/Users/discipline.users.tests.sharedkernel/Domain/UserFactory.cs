using Bogus;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.users.domain.Users;

namespace discipline.users.tests.sharedkernel.Domain;

public static class UserFactory
{
    public static User Get()
        => Get(1).Single();
    
    private static List<User> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<User> GetFaker()
        => new Faker<User>().CustomInstantiator(v => User.Create(
            UserId.New(), 
            v.Internet.Email(),
                v.Random.String(minChar:'a', maxChar:'z'),
            v.Name.FirstName(),
            v.Name.LastName()));
}