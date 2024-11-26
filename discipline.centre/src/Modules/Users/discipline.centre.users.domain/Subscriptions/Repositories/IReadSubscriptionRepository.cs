using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.domain.Subscriptions.Repositories;

public interface IReadSubscriptionRepository
{
    Task<bool> DoesTitleExistAsync(string title, CancellationToken cancellationToken = default);
    Task<Subscription?> GetByIdAsync(SubscriptionId id, CancellationToken cancellationToken = default);
}