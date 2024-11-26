namespace discipline.centre.users.domain.Subscriptions.Repositories;

public interface IWriteSubscriptionRepository
{
    Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default);
}