using Bogus;
using discipline.application.Domain.Users.Entities;

namespace discipline.tests.shared.Entities;

internal static class FreeSubscriptionOrderFactory
{
    internal static FreeSubscriptionOrder Get(Subscription subscription)
        => Get(1, subscription).Single();
    
    private static List<FreeSubscriptionOrder> Get(int count, Subscription subscription)
        => GetFaker(subscription).Generate(count);
    
    private static Faker<FreeSubscriptionOrder> GetFaker(Subscription subscription)
        => new Faker<FreeSubscriptionOrder>().CustomInstantiator(v
            => FreeSubscriptionOrder.Create(Guid.NewGuid(), subscription, DateTime.Now));
}