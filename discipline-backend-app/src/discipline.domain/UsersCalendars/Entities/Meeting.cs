using discipline.domain.SharedKernel;
using discipline.domain.UsersCalendars.ValueObjects.Event;

namespace discipline.domain.UsersCalendars.Entities;

public sealed class Meeting : Event
{
    public MeetingTimeSpan MeetingTimeSpan { get; private set; }
    public Address Address { get; private set; }


    private Meeting(EntityId id, Title title) : base(id, title)
    {
    }

    //For mongo
    public Meeting(EntityId id, Title title, MeetingTimeSpan meetingTimeSpan, Address address) : base(id, title)
    {
        MeetingTimeSpan = meetingTimeSpan;
        Address = address;
    }

    internal static Meeting Create(Guid id, string title, TimeOnly timeFrom, TimeOnly? timeTo,
        string platform, string uri, string place)
    {
        var @event = new Meeting(id, title);
        @event.ChangeMeetingTimeSpan(timeFrom, timeTo);
        @event.ChangeMeetingAddress(platform, uri, place);
        return @event;
    }

    private void ChangeMeetingTimeSpan(TimeOnly timeFrom, TimeOnly? timeTo)
        => MeetingTimeSpan = new MeetingTimeSpan(timeFrom, timeTo);

    private void ChangeMeetingAddress(string platform, string uri, string place)
        => Address = new Address(platform, uri, place);
}