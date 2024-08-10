using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.domain.DailyProductivities.Entities;
using discipline.domain.DailyProductivities.Repositories;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class MongoDailyProductivityRepository(
    IDisciplineMongoCollection disciplineMongoCollection) : IDailyProductivityRepository
{
    private readonly IMongoCollection<DailyProductivityDocument> _collection =
        disciplineMongoCollection.GetCollection<DailyProductivityDocument>();

    public Task AddAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default)
        => _collection.InsertOneAsync(dailyProductivity.AsDocument(), null, cancellationToken);

    public Task UpdateAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default)
        => _collection.FindOneAndReplaceAsync(x => x.Day.Equals(dailyProductivity.Day),
            dailyProductivity.AsDocument(), null, cancellationToken);

    public async Task<DailyProductivity> GetByDateAsync(DateOnly day, CancellationToken cancellationToken = default)
        => (await _collection.Find(x => x.Day == day)
                .FirstOrDefaultAsync(cancellationToken))?
            .AsEntity();

    public async Task<DailyProductivity> GetByActivityId(Guid activityId, CancellationToken cancellationToken = default)
        => (await _collection.Find(x 
                    => x.Activities != null && x.Activities.Any(a => a.Id.Equals(activityId)))
                .FirstOrDefaultAsync(cancellationToken))?
            .AsEntity();
}