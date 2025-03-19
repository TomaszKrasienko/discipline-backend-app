using discipline.centre.calendar.domain;
using discipline.centre.calendar.domain.Repositories;
using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.calendar.infrastructure.DAL.Calendar.Documents;
using discipline.centre.calendar.infrastructure.DAL.Documents;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using MongoDB.Driver;

namespace discipline.centre.calendar.infrastructure.DAL.Repositories;

internal sealed class MongoUserCalendarDayRepository(
    CalendarMongoContext context) : IReadUserCalendarRepository, IReadWriteUserCalendarRepository
{
    public async Task<UserCalendarDay?> GetByDayAsync(UserId userId, Day day, CancellationToken cancellationToken)
        => (await context.GetCollection<UserCalendarDayDocument>()
            .Find(x 
                => x.Day == day.Value
                && x.UserId == userId.ToString())
            .SingleOrDefaultAsync(cancellationToken)).AsEntity();

    public Task AddAsync(UserCalendarDay userCalendarDay, CancellationToken cancellationToken)
        => context.GetCollection<UserCalendarDayDocument>()
            .InsertOneAsync(userCalendarDay.AsDocument(), null, cancellationToken);

    public Task UpdateAsync(UserCalendarDay userCalendarDay, CancellationToken cancellationToken)
        => context.GetCollection<UserCalendarDayDocument>()
            .FindOneAndReplaceAsync(x => x.UserCalendarId == userCalendarDay.Id.ToString(), userCalendarDay.AsDocument(),
                null, cancellationToken);
}