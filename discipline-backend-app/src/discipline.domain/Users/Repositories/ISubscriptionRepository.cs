using discipline.domain.Users.Entities;

namespace discipline.domain.Users.Repositories;

public interface ISubscriptionRepository
{
    Task<Subscription> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default);
}