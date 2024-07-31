using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Repositories;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class MongoSubscriptionRepository(
    IDisciplineMongoCollection disciplineMongoCollection) : ISubscriptionRepository
{
    public async Task<Subscription> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<SubscriptionDocument>()
            .Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken))?.AsEntity();

    public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
        => await disciplineMongoCollection.GetCollection<SubscriptionDocument>()
            .InsertOneAsync(subscription.AsDocument(), null, cancellationToken);
}