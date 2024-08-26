using discipline.domain.SharedKernel;
using discipline.domain.UsersCalendars.Policies;

namespace discipline.domain.UsersCalendars.ValueObjects.Event;

public sealed class Address : ValueObject
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

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Platform;
        yield return Uri;
        yield return Place;
    }
}