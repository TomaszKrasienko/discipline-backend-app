using Bogus;
using discipline.application.Infrastructure.DAL.Documents.Users;

namespace discipline.tests.shared.Documents;

internal static class FreeSubscriptionOrderDocumentFactory
{
    internal static FreeSubscriptionOrderDocument Get()
        => Get(1).Single();
    
    private static List<FreeSubscriptionOrderDocument> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<FreeSubscriptionOrderDocument> GetFaker()
        => new Faker<FreeSubscriptionOrderDocument>()
            .RuleFor(f => f.Id, Ulid.NewUlid())
            .RuleFor(f => f.CreatedAt, DateTime.Now)
            .RuleFor(f => f.SubscriptionId, Ulid.NewUlid())
            .RuleFor(f => f.StateIsCancelled, v => v.PickRandom<bool>(true, false))
            .RuleFor(f => f.StateActiveTill, v => null);
}