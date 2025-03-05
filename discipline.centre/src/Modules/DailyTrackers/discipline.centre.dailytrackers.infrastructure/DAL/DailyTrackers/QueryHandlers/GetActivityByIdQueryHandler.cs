using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.application.DailyTrackers.DTOs.Responses;
using discipline.centre.dailytrackers.application.DailyTrackers.Queries;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.shared.abstractions.CQRS.Queries;
using MongoDB.Driver;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.QueryHandlers;

/// <inheritdoc cref="GetActivityByIdQuery"/>
internal sealed class GetActivityByIdQueryHandler(
    DailyTrackersMongoContext dailyTrackersMongoContext) : IQueryHandler<GetActivityByIdQuery, ActivityDto?>
{
    public async Task<ActivityDto?> HandleAsync(GetActivityByIdQuery query, CancellationToken cancellationToken = default)
    {
        var result = await dailyTrackersMongoContext
            .GetCollection<DailyTrackerDocument>()
            .Find(x
                => x.UserId == query.UserId.ToString()
                   && x.Activities.Any(y => y.ActivityId == query.ActivityId.ToString()))
            .SingleOrDefaultAsync(cancellationToken);
        
        return result?
            .Activities?
            .SingleOrDefault(x => x.ActivityId == query.ActivityId.ToString())?.AsDto();   
    }
}