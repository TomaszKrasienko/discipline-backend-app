namespace discipline.infrastructure.Events.Publisher;

internal interface IEventsChannelConventionProvider
{
    string Get(Type eventType);
}