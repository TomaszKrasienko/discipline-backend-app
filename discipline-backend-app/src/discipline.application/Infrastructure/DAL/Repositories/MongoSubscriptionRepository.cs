using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Repositories;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class MongoSubscriptionRepository : ISubscriptionRepository
{
    public Task<Subscription> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}