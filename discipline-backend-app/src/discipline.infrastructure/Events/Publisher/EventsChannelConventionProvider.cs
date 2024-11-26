using Humanizer;

namespace discipline.infrastructure.Events.Publisher;

//TODO: Tests
internal sealed class EventsChannelConventionProvider : IEventsChannelConventionProvider
{
    public string Get(Type eventType)
        => eventType.Name.Underscore();
}