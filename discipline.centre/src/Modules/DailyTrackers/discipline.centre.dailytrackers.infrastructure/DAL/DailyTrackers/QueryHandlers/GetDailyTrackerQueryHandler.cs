using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.application.DailyTrackers.Queries;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.shared.abstractions.CQRS.Queries;
using MongoDB.Driver;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.QueryHandlers;

internal sealed class GetDailyTrackerQueryHandler(
    DailyTrackersMongoContext dailyTrackersMongoContext) : IQueryHandler<GetDailyTrackerQuery, DailyTrackerDto>
{
    public async Task<DailyTrackerDto> HandleAsync(GetDailyTrackerQuery query, CancellationToken cancellationToken = default)
    {
        var dailyTracker = await dailyTrackersMongoContext
            .GetCollection<DailyTrackerDocument>()
            .Find(x => x.Day == query.Day)
            .SingleOrDefaultAsync(cancellationToken);

        return dailyTracker.AsDto();
    }
}