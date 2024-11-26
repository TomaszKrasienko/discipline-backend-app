using System.Linq.Expressions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;
using MongoDB.Driver;

namespace discipline.centre.users.infrastructure.DAL.Subscriptions.Repositories;

internal sealed class MongoSubscriptionRepository(
    UsersMongoContext context) : IWriteSubscriptionRepository, IReadSubscriptionRepository
{
    public Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
        => context.GetCollection<SubscriptionDocument>()
            .InsertOneAsync(subscription.MapAsDocument(), null, cancellationToken);

    public Task<bool> DoesTitleExistAsync(string title, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Title == title, cancellationToken);
    
    private Task<bool> AnyAsync(Expression<Func<SubscriptionDocument, bool>> expression, CancellationToken cancellationToken = default)
        => context
            .GetCollection<SubscriptionDocument>()
            .Find(expression.ToFilterDefinition())
            .AnyAsync(cancellationToken);

    public async Task<Subscription?> GetByIdAsync(SubscriptionId id, CancellationToken cancellationToken = default)
        => (await GetAsync(x => x.Id == id.Value.ToString(), cancellationToken))?.MapAsEntity();
    
    private async Task<SubscriptionDocument?> GetAsync(Expression<Func<SubscriptionDocument, bool>> expression, CancellationToken cancellationToken = default)
        => await context.GetCollection<SubscriptionDocument>()
            .Find(expression.ToFilterDefinition())
            .SingleOrDefaultAsync(cancellationToken);
}