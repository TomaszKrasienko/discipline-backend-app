using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.infrastructure.Events.Brokers.Internal.Abstractions;
using discipline.centre.shared.infrastructure.Events.Brokers.Internal.Channels;

namespace discipline.centre.shared.infrastructure.Events.Brokers.Internal;

internal sealed class AsyncMessageDispatcher(IMessageChannel messageChannel) : IAsyncMessageDispatcher
{
    public async Task PublishAsync<T>(T message) where T : class, IEvent
        => await messageChannel.Writer.WriteAsync(message);
}