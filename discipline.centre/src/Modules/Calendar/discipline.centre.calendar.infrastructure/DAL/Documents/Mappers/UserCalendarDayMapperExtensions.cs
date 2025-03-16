using discipline.centre.calendar.infrastructure.DAL.Calendar.Documents;
using discipline.centre.calendar.infrastructure.DAL.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.calendar.domain;

internal static class UserCalendarDayMapperExtensions
{
    internal static UserCalendarDayDocument AsDocument(this UserCalendarDay entity)
        => new()
        {
            UserCalendarId = entity.Id.ToString(),
            UserId = entity.UserId.ToString(),
            Day = entity.Day,
            Events = entity.Events.Select(x => x.AsDocument()).ToArray(),
        };

    private static BaseCalendarEventDocument AsDocument(this BaseCalendarEvent entity) => entity switch
    {
        TimeEvent timeEvent => timeEvent.AsDocument(),
        ImportantDateEvent importantDateEvent => importantDateEvent.AsDocument(),
        _ => throw new ArgumentOutOfRangeException(nameof(entity))
    };

    private static TimeEventDocument AsDocument(this TimeEvent @event)
        => new()
        {
            EventId = @event.Id.ToString(),
            Content = new()
            {
                Title = @event.Content.Title,
                Description = @event.Content.Description,
            },
            TimeSpan = new()
            {
                From = @event.TimeSpan.From,
                To = @event.TimeSpan.To
            }
        };

    private static ImportantDateEventDocument AsDocument(this ImportantDateEvent @event)
        => new()
        {
            EventId = @event.Id.ToString(),
            Content = new()
            {
                Title = @event.Content.Title,
                Description = @event.Content.Description
            }
        };
}