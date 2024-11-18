namespace discipline.centre.users.domain.Subscriptions.Repositories;

public interface IReadSubscriptionRepository
{
    Task<bool> DoesTitleExistAsync(string title, CancellationToken cancellationToken = default);
}