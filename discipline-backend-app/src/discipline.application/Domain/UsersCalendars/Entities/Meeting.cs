using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.UsersCalendars.ValueObjects.Event;

namespace discipline.application.Domain.UsersCalendars.Entities;

internal sealed class Meeting : Event
{
    public MeetingTimeSpan MeetingTimeSpan { get; private set; }
    public Address Address { get; private set; }


    private Meeting(EntityId id, Title title, EventDay eventDay) : base(id, title, eventDay)
    {
    }

    internal static Meeting Create(Guid id, string title, DateOnly eventDay, TimeOnly timeFrom, TimeOnly? timeTo,
        string platform, string uri, string place)
    {
        var @event = new Meeting(id, title, eventDay);
        @event.ChangeMeetingTimeSpan(timeFrom, timeTo);
        @event.ChangeMeetingAddress(platform, uri, place);
        return @event;
    }

    private void ChangeMeetingTimeSpan(TimeOnly timeFrom, TimeOnly? timeTo)
        => MeetingTimeSpan = new MeetingTimeSpan(timeFrom, timeTo);

    private void ChangeMeetingAddress(string platform, string uri, string place)
        => Address = new Address(platform, uri, place);
}