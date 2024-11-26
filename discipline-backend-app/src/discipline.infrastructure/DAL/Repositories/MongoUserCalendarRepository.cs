using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Repositories;
using discipline.infrastructure.DAL.Connection;
using discipline.infrastructure.DAL.Documents.Mappers;
using discipline.infrastructure.DAL.Documents.UsersCalendar;
using MongoDB.Driver;

namespace discipline.infrastructure.DAL.Repositories;

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

    public async Task<UserCalendar> GetForUserByDateAsync(UserId userId, DateOnly day, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<UserCalendarDocument>()
            .Find(x => x.Day == day && x.UserId == userId.ToString()).FirstOrDefaultAsync(cancellationToken))?.AsEntity();

    public async Task<UserCalendar> GetByEventIdAsync(UserId userId, EventId eventId, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<UserCalendarDocument>()
            .Find(x => x.UserId == userId.ToString() && x.Events.Any(y => y.Id == eventId.ToString()))
            .FirstOrDefaultAsync(cancellationToken))?.AsEntity();
}