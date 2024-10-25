using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users;
using discipline.domain.Users.Repositories;
using discipline.infrastructure.DAL.Connection;
using discipline.infrastructure.DAL.Documents.Mappers;
using discipline.infrastructure.DAL.Documents.Users;
using MongoDB.Driver;

namespace discipline.infrastructure.DAL.Repositories;

internal sealed class MongoSubscriptionRepository(
    IDisciplineMongoCollection disciplineMongoCollection) : ISubscriptionRepository
{
    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<SubscriptionDocument>()
            .Find(_ => true).AnyAsync(cancellationToken));

    public async Task<Subscription> GetByIdAsync(SubscriptionId id, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<SubscriptionDocument>()
            .Find(x => x.Id == id.Value.ToString())
            .FirstOrDefaultAsync(cancellationToken))?.AsEntity();

    public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
        => await disciplineMongoCollection.GetCollection<SubscriptionDocument>()
            .InsertOneAsync(subscription.AsDocument(), null, cancellationToken);
}