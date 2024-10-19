using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;

namespace discipline.domain.Users.Repositories;

public interface ISubscriptionRepository
{
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<Subscription> GetByIdAsync(SubscriptionId id, CancellationToken cancellationToken = default);
    Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default);
}