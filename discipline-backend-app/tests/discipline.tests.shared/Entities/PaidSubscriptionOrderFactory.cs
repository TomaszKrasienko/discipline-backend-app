using Bogus;
using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Enums;

namespace discipline.tests.shared.Entities;

internal static class PaidSubscriptionOrderFactory
{
    internal static PaidSubscriptionOrder Get(Subscription subscription)
        => Get(1, subscription).Single();
    
    private static List<PaidSubscriptionOrder> Get(int count, Subscription subscription)
        => GetFaker(subscription).Generate(count);
    
    private static Faker<PaidSubscriptionOrder> GetFaker(Subscription subscription)
        => new Faker<PaidSubscriptionOrder>()
            .CustomInstantiator(v => PaidSubscriptionOrder.Create(
                Guid.NewGuid(), subscription, v.PickRandom<SubscriptionOrderFrequency>(), DateTime.Now,
                v.Random.String(minLength: 13, maxLength: 19), v.Random.String(length: 3)));
}