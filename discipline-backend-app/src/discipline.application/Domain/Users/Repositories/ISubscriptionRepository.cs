using discipline.application.Domain.Users.Entities;

namespace discipline.application.Domain.Users.Repositories;

internal interface ISubscriptionRepository
{
    Task<Subscription> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default);
}