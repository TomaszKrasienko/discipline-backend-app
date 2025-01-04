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
            .SingleOrDefaultAsync(cancellationToken))?.MapAsEntity();

    public Task AddAsync(DailyTracker dailyTracker, CancellationToken cancellationToken = default)
        => context.GetCollection<DailyTrackerDocument>()
            .InsertOneAsync(dailyTracker.MapAsDocument(), cancellationToken: cancellationToken);

    public Task UpdateAsync(DailyTracker dailyTracker, CancellationToken cancellationToken = default)
        => context.GetCollection<DailyTrackerDocument>()
            .FindOneAndReplaceAsync(x => x.DailyTrackerId == dailyTracker.Id.ToString(),
                dailyTracker.MapAsDocument(), null, cancellationToken);
}