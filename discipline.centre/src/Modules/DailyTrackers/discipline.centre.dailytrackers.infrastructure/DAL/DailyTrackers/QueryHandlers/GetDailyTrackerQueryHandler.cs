using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.application.DailyTrackers.DTOs.Responses;
using discipline.centre.dailytrackers.application.DailyTrackers.Queries;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.CacheDecorators;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.abstractions.CQRS.Queries;
using MongoDB.Driver;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.QueryHandlers;

internal sealed class GetDailyTrackerQueryHandler(
    DailyTrackersMongoContext dailyTrackersMongoContext,
    ICacheFacade cacheFacade) : IQueryHandler<GetDailyTrackerByDayQuery, DailyTrackerDto?>
{
    public async Task<DailyTrackerDto?> HandleAsync(GetDailyTrackerByDayQuery query, CancellationToken cancellationToken = default)
    {
        var key = CacheDailyTrackerRepositoryDecorator.GetCacheKey(query.UserId, query.Day.ToString());
        DailyTrackerDocument? document = await cacheFacade.GetAsync<DailyTrackerDocument>(key, cancellationToken);

        if (document is null)
        {
            document = await dailyTrackersMongoContext
                .GetCollection<DailyTrackerDocument>()
                .Find(x => x.Day == query.Day)
                .SingleOrDefaultAsync(cancellationToken);
            
            await cacheFacade.AddOrUpdateAsync(key, document, cancellationToken);
        }

        return document?.AsDto();
    }
}