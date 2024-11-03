using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Subscriptions.DTOs;
using discipline.centre.users.domain.Subscriptions.ValueObjects;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.domain.Subscriptions;

internal static class SubscriptionDocumentsMappingExtensions
{
    internal static Subscription MapAsEntity(this SubscriptionDocument document)
        => new (
            SubscriptionId.Parse(document.Id),
            document.Title,
            Price.Create(document.PricePerMonth, document.PricePerYear),
            document.Features.Select(Feature.Create).ToList());
    
    internal static SubscriptionDto MapAsDto(this SubscriptionDocument document)
        => new()
        {
            Id = Ulid.Parse(document.Id),
            Title = document.Title,
            PricePerMonth = document.PricePerMonth,
            PricePerYear = document.PricePerYear,
            IsPaid = document.IsPaid,
            Features = document.Features
        };
}
