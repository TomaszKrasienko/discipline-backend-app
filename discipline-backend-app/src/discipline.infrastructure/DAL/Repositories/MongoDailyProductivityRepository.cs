using discipline.domain.DailyProductivities.Entities;
using discipline.domain.DailyProductivities.Repositories;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.infrastructure.DAL.Connection;
using discipline.infrastructure.DAL.Documents;
using discipline.infrastructure.DAL.Documents.DailyProductivities;
using discipline.infrastructure.DAL.Documents.Mappers;
using MongoDB.Driver;

namespace discipline.infrastructure.DAL.Repositories;

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

    public async Task<DailyProductivity> GetByActivityId(ActivityId activityId, CancellationToken cancellationToken = default)
        => (await _collection.Find(x 
                    => x.Activities != null && x.Activities.Any(a => a.Id == activityId.ToString()))
                .FirstOrDefaultAsync(cancellationToken))?
            .AsEntity();
}