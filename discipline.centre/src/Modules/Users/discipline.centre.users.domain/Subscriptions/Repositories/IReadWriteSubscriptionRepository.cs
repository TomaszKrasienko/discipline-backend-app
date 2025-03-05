namespace discipline.centre.users.domain.Subscriptions.Repositories;

public interface IReadWriteSubscriptionRepository : IReadSubscriptionRepository
{
    Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default);
}