using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Repositories;
using discipline.application.Infrastructure.DAL.Connection;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class MongoSubscriptionRepository(
    IDisciplineMongoCollection disciplineMongoCollection) : ISubscriptionRepository
{
    public Task<Subscription> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}