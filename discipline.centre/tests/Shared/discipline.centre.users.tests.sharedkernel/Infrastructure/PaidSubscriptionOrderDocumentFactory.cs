using Bogus;
using discipline.centre.users.infrastructure.DAL.Users.Documents;

namespace discipline.centre.users.tests.sharedkernel.Infrastructure;

internal static class PaidSubscriptionOrderDocumentFactory
{
    internal static PaidSubscriptionOrderDocument Get()
        => Get(1).Single();
    
    private static List<PaidSubscriptionOrderDocument> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<PaidSubscriptionOrderDocument> GetFaker()
        => new Faker<PaidSubscriptionOrderDocument>()
            .RuleFor(f => f.Id, Ulid.NewUlid().ToString())
            .RuleFor(f => f.CreatedAt, DateTime.Now)
            .RuleFor(f => f.SubscriptionId, Ulid.NewUlid())
            .RuleFor(f => f.StateIsCancelled, v => v.PickRandom<bool>(true, false))
            .RuleFor(f => f.StateActiveTill, DateOnly.FromDateTime(DateTime.Now.AddMonths(1)))
            .RuleFor(f => f.Next, DateOnly.FromDateTime(DateTime.Now.AddMonths(2)))
            .RuleFor(f => f.PaymentToken, v => Guid.NewGuid().ToString());
}