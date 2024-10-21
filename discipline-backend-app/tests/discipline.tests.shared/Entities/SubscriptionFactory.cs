using Bogus;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;

namespace discipline.tests.shared.Entities;

internal sealed class SubscriptionFactory
{
    internal static Subscription Get(decimal perMonth = 0, decimal perYear = 0)
        => Get(1, perMonth, perYear).Single();
    
    private static List<Subscription> Get(int count, decimal perMonth = 0, decimal perYear = 0)
        => GetFaker(perMonth, perYear).Generate(count);
    
    private static Faker<Subscription> GetFaker(decimal perMonth = 0, decimal perYear = 0)
        => new Faker<Subscription>().CustomInstantiator(v
            => Subscription.Create(SubscriptionId.New(), v.Lorem.Word(), perMonth, perYear, [v.Random.String(minChar:'a', maxChar:'z')]));
}