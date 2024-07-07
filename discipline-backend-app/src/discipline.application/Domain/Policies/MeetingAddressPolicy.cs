using discipline.application.Domain.Exceptions;
using discipline.application.Domain.Policies.Abstractions;

namespace discipline.application.Domain.Policies;

internal sealed class MeetingAddressPolicy : IPolicy
{
    private readonly string _platform;
    private readonly string _uri;
    private readonly string _place;

    private MeetingAddressPolicy(string platform, string uri, string place)
    {
        _platform = platform;
        _uri = uri;
        _place = place;
    }

    internal static MeetingAddressPolicy GetInstance(string platform, string uri, string place)
        => new MeetingAddressPolicy(platform, uri, place);
    
    public void Validate()
    {
        if(IsPlaceIsEmpty() && IsUriIsEmpty() && IsPlatformIsEmpty())
        {
            throw new EmptyAddressException();
        }

        if (!IsPlaceIsEmpty() && (!IsPlatformIsEmpty() || !IsUriIsEmpty()))
        {
            throw new InconsistentAddressTypeException();
        }
    }

    private bool IsPlatformIsEmpty()
        => string.IsNullOrWhiteSpace(_platform);
    
    private bool IsUriIsEmpty()
        => string.IsNullOrWhiteSpace(_uri);
    
    private bool IsPlaceIsEmpty()
        => string.IsNullOrWhiteSpace(_place);
}
