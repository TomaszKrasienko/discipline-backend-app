using Bogus;
using discipline.infrastructure.DAL.Documents.Users;

namespace discipline.tests.shared.Documents;

public static class SubscriptionDocumentFactory
{
    internal static SubscriptionDocument Get(decimal perMonth = 0, decimal perYear = 0)
        => Get(1, perMonth, perYear).Single();
    
    private static List<SubscriptionDocument> Get(int count, decimal perMonth = 0, decimal perYear = 0)
        => GetFaker(perMonth, perYear).Generate(count);
    
    private static Faker<SubscriptionDocument> GetFaker(decimal perMonth = 0, decimal perYear = 0)
        => new Faker<SubscriptionDocument>()
            .RuleFor(f => f.Id, Ulid.NewUlid().ToString())
            .RuleFor(f => f.Title, v => v.Lorem.Word())
            .RuleFor(f => f.PricePerMonth, perMonth)
            .RuleFor(f => f.PricePerYear, perYear)
            .RuleFor(f => f.Features, v => [v.Random.String(minChar:'a', maxChar:'z')]);
}