using Bogus;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;

namespace discipline.centre.users.tests.sharedkernel.Domain;

public static class SubscriptionFakeDataFactory
{
    public static Subscription Get(decimal perMonth = 0, decimal perYear = 0)
        => Get(1, perMonth, perYear).Single();
    
    private static List<Subscription> Get(int count, decimal perMonth = 0, decimal perYear = 0)
        => GetFaker(perMonth, perYear).Generate(count);
    
    private static Faker<Subscription> GetFaker(decimal perMonth = 0, decimal perYear = 0)
        => new Faker<Subscription>().CustomInstantiator(v => Subscription.Create(
            SubscriptionId.New(),
            v.Lorem.Word(),
            perMonth, 
            perYear,
            [v.Random.String(minChar:'a', maxChar:'z')]));
}