using discipline.application.Behaviours;
using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Domain.UsersCalendars.Repositories;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class MongoUserCalendarRepository(
    IDisciplineMongoCollection disciplineMongoCollection) : IUserCalendarRepository
{
    public async Task AddAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default)
        => await disciplineMongoCollection.GetCollection<UserCalendarDocument>()
            .InsertOneAsync(userCalendar?.AsDocument(), null, cancellationToken);

    public async Task UpdateAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default)
        => await disciplineMongoCollection.GetCollection<UserCalendarDocument>()
            .FindOneAndReplaceAsync(x => x.Day == userCalendar.Day.Value, 
                userCalendar.AsDocument(), null, cancellationToken);

    public async Task<UserCalendar> GetByDateAsync(DateOnly day, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == day).FirstOrDefaultAsync(cancellationToken))?.AsEntity();
}