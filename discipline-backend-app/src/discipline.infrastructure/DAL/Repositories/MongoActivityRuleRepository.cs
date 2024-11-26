using discipline.domain.ActivityRules.Entities;
using discipline.domain.ActivityRules.Repositories;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.infrastructure.DAL.Connection;
using discipline.infrastructure.DAL.Documents;
using discipline.infrastructure.DAL.Documents.ActivityRules;
using discipline.infrastructure.DAL.Documents.Mappers;
using MongoDB.Driver;

namespace discipline.infrastructure.DAL.Repositories;

internal sealed class MongoActivityRuleRepository(
    IDisciplineMongoCollection disciplineMongoCollection) : IActivityRuleRepository
{
    private readonly IMongoCollection<ActivityRuleDocument> _collection =
        disciplineMongoCollection.GetCollection<ActivityRuleDocument>(); 

    public Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => _collection.InsertOneAsync(activityRule.AsDocument(), null, cancellationToken);

    public Task UpdateAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => _collection.FindOneAndReplaceAsync(x => x.Id == activityRule.Id.ToString(),
            activityRule.AsDocument(), null, cancellationToken);

    public Task DeleteAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => _collection.FindOneAndDeleteAsync(x => x.Id == activityRule.Id.ToString(),
            null, cancellationToken);

    public Task<bool> ExistsAsync(string title, CancellationToken cancellationToken = default)
        => _collection.Find(x => x.Title == title).AnyAsync(cancellationToken);

    public async Task<ActivityRule> GetByIdAsync(ActivityRuleId id, CancellationToken cancellationToken = default)
        => (await _collection.Find(x => x.Id == id.Value.ToString())
            .FirstOrDefaultAsync(cancellationToken))?
            .AsEntity();

    public async Task<List<ActivityRule>> BrowseAsync(CancellationToken cancellationToken = default)
        => (await _collection.Find(_ => true).ToListAsync(cancellationToken))?
            .Select(x => x.AsEntity())
            .ToList();
}