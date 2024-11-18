using System.Linq.Expressions;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;
using MongoDB.Driver;

namespace discipline.centre.users.infrastructure.DAL.Subscriptions.Repositories;

internal sealed class MongoSubscriptionRepository(
    IMongoCollectionContext context) : IWriteSubscriptionRepository, IReadSubscriptionRepository
{
    public Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
        => context.GetCollection<SubscriptionDocument>()
            .InsertOneAsync(subscription.AsDocument(), null, cancellationToken);

    public Task<bool> DoesTitleExistAsync(string title, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Title == title, cancellationToken);
    
    private Task<bool> AnyAsync(Expression<Func<SubscriptionDocument, bool>> expression, CancellationToken cancellationToken = default)
        => context
            .GetCollection<SubscriptionDocument>()
            .Find(expression.ToFilterDefinition())
            .AnyAsync(cancellationToken);
}