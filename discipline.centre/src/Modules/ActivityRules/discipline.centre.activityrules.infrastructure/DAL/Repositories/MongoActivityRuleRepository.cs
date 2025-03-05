using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using MongoDB.Driver;

namespace discipline.centre.activityrules.infrastructure.DAL.Repositories;

internal sealed class MongoActivityRuleRepository(
    ActivityRulesMongoContext context) : IReadWriteActivityRuleRepository 
{
    public Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => context.GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRule.MapAsDocument(),
                null,
                cancellationToken);

    public async Task UpdateAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => await context.GetCollection<ActivityRuleDocument>()
            .FindOneAndReplaceAsync(x 
                    => x.Id == activityRule.Id.ToString(),
                activityRule.MapAsDocument(),
                null,
                cancellationToken);

    public Task DeleteAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => context.GetCollection<ActivityRuleDocument>()
            .FindOneAndDeleteAsync(x => x.Id == activityRule.Id.ToString(),
            null, cancellationToken);

    public Task<bool> ExistsAsync(string title, UserId userId, CancellationToken cancellationToken = default)
        => context.GetCollection<ActivityRuleDocument>()
            .Find(x 
                => x.Title == title
                && x.UserId == userId.ToString()).AnyAsync(cancellationToken);

    public async Task<ActivityRule?> GetByIdAsync(ActivityRuleId id, UserId userId, CancellationToken cancellationToken = default)
        => (await context.GetCollection<ActivityRuleDocument>()
                .Find(x 
                    => x.Id == id.Value.ToString()
                    && x.UserId == userId.ToString())
                .FirstOrDefaultAsync(cancellationToken))?
            .MapAsEntity();
}