using discipline.application.Domain.ActivityRules;
using discipline.application.Domain.ActivityRules.Entities;
using discipline.application.Domain.ActivityRules.Repositories;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class MongoActivityRuleRepository(
    IDisciplineMongoCollection disciplineMongoCollection) : IActivityRuleRepository
{
    private readonly IMongoCollection<ActivityRuleDocument> _collection =
        disciplineMongoCollection.GetCollection<ActivityRuleDocument>(); 

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

    public async Task<List<ActivityRule>> BrowseAsync(CancellationToken cancellationToken = default)
        => (await _collection.Find(_ => true).ToListAsync(cancellationToken))?
            .Select(x => x.AsEntity())
            .ToList();
}