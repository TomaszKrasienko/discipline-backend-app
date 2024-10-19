using Bogus;
using discipline.application.Infrastructure.DAL.Documents.Users;

namespace discipline.tests.shared.Documents;

internal static class PaidSubscriptionOrderDocumentFactory
{
    internal static PaidSubscriptionOrderDocument Get()
        => Get(1).Single();
    
    private static List<PaidSubscriptionOrderDocument> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<PaidSubscriptionOrderDocument> GetFaker()
        => new Faker<PaidSubscriptionOrderDocument>()
            .RuleFor(f => f.Id, Ulid.NewUlid())
            .RuleFor(f => f.CreatedAt, DateTime.Now)
            .RuleFor(f => f.SubscriptionId, Ulid.NewUlid())
            .RuleFor(f => f.StateIsCancelled, v => v.PickRandom<bool>(true, false))
            .RuleFor(f => f.StateActiveTill, DateOnly.FromDateTime(DateTime.Now.AddMonths(1)))
            .RuleFor(f => f.Next, DateOnly.FromDateTime(DateTime.Now.AddMonths(2)))
            .RuleFor(f => f.PaymentDetailsCardNumber, v => v.Random.String(minLength: 14, maxLength: 19))
            .RuleFor(f => f.PaymentDetailsCvvCode, v => v.Random.String(length: 3));
}