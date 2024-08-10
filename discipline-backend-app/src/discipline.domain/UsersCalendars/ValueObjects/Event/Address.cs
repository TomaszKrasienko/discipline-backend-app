using discipline.domain.UsersCalendars.Policies;

namespace discipline.domain.UsersCalendars.ValueObjects.Event;

public sealed record Address
{
    public string Platform { get; }
    public string Uri { get; }
    public string Place { get; }

    public Address(string platform, string uri, string place)
    {
        var policy = MeetingAddressPolicy.GetInstance(platform, uri, place);
        policy.Validate();

        Platform = platform;
        Uri = uri;
        Place = place;
    }
}