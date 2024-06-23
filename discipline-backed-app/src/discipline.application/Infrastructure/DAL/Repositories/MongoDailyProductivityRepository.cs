using discipline.application.Domain.Entities;
using discipline.application.Domain.Repositories;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class MongoDailyProductivityRepository(IMongoDatabase mongoDatabase) : IDailyProductivityRepository
{
    internal const string CollectionName = "DailyProductivity";
    private readonly IMongoCollection<DailyProductivityDocument> _collection = mongoDatabase.GetCollection<DailyProductivityDocument>(CollectionName);

    public Task AddAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default)
        => _collection.InsertOneAsync(dailyProductivity.AsDocument(), null, cancellationToken);

    public Task UpdateAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default)
        => _collection.FindOneAndReplaceAsync(x => x.Day.Equals(dailyProductivity.Day),
            dailyProductivity.AsDocument(), null, cancellationToken);

    public async Task<DailyProductivity> GetByDateAsync(DateOnly day, CancellationToken cancellationToken = default)
        => (await _collection.Find(x => x.Day == day)
                .FirstOrDefaultAsync(cancellationToken))?
            .AsEntity();
}