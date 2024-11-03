using discipline.centre.users.domain.Subscriptions;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;

internal static class SubscriptionEntitiesMappingExtensions
{
    internal static SubscriptionDocument AsDocument(this Subscription entity)
        => new()
        {
            Id = entity.Id.ToString(),
            PricePerMonth = entity.Price.PerMonth,
            PricePerYear = entity.Price.PerYear,
            Title = entity.Title,
            IsPaid = !entity.IsFree(),
            Features = entity.Features.Select(x => x.Value).ToList()
        };
}