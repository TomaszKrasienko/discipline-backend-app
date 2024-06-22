using discipline.application.Domain.Entities;
using discipline.application.Domain.Repositories;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class MongoActivityRuleRepository : IActivityRuleRepository
{
    private const string CollectionName = "ActivityRules";
    private readonly IMongoCollection<ActivityRuleDocument> _collection;

    public MongoActivityRuleRepository(IMongoDatabase mongoDatabase)
        => _collection = mongoDatabase.GetCollection<ActivityRuleDocument>(CollectionName);

    public Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => _collection.InsertOneAsync(activityRule.AsDocument(), null, cancellationToken);

    public Task UpdateAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => _collection.FindOneAndReplaceAsync(x => x.Id.Equals(activityRule.Id),
            activityRule.AsDocument(), null, cancellationToken);

    public Task DeleteAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => _collection.FindOneAndDeleteAsync(x => x.Id.Equals(activityRule.Id),
            null, cancellationToken);

    public Task<bool> ExistsAsync(string title, CancellationToken cancellationToken = default)
        => _collection.Find(x => x.Title == title).AnyAsync(cancellationToken);

    public async Task<ActivityRule> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await _collection.Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken))?
            .AsEntity();
}