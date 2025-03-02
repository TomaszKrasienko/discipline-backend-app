using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using MongoDB.Driver;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Repositories;

internal sealed class MongoDailyTrackerRepository(
    DailyTrackersMongoContext context) : IWriteReadDailyTrackerRepository
{
    public async Task<DailyTracker?> GetDailyTrackerByDayAsync(DateOnly day, UserId userId,
        CancellationToken cancellationToken = default)
        => (await context.GetCollection<DailyTrackerDocument>().Find(x
                => x.Day == day
                && x.UserId == userId.ToString())
            .SingleOrDefaultAsync(cancellationToken))?.AsEntity();

    public async Task<DailyTracker?> GetDailyTrackerByIdAsync(DailyTrackerId id, UserId userId,
        CancellationToken cancellationToken = default)
        => (await context.GetCollection<DailyTrackerDocument>().Find(x
                => x.DailyTrackerId == id.ToString() 
                && x.UserId == userId.ToString())
            .SingleOrDefaultAsync(cancellationToken))?.AsEntity();

    public async Task<List<DailyTracker>> GetDailyTrackersByParentActivityRuleId(ActivityRuleId activityRuleId,
        UserId userId,
        CancellationToken cancellationToken = default)
        => (await context.GetCollection<DailyTrackerDocument>()
                .Find(dt => dt.Activities.Any(activity
                                => activity.ParentActivityRuleId == activityRuleId.ToString())
                            && dt.UserId == userId.ToString())
                .ToListAsync(cancellationToken))
            .Select(x => x.AsEntity()).ToList();
        

    public Task AddAsync(DailyTracker dailyTracker, CancellationToken cancellationToken = default)
        => context.GetCollection<DailyTrackerDocument>()
            .InsertOneAsync(dailyTracker.AsDocument(), cancellationToken: cancellationToken);

    public Task UpdateAsync(DailyTracker dailyTracker, CancellationToken cancellationToken = default)
        => context.GetCollection<DailyTrackerDocument>()
            .FindOneAndReplaceAsync(x => x.DailyTrackerId == dailyTracker.Id.ToString(),
                dailyTracker.AsDocument(), null, cancellationToken);
}