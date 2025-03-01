using System.Threading.Channels;
using discipline.centre.shared.abstractions.Events;

namespace discipline.centre.shared.infrastructure.Events.Channels;

internal interface IMessageChannel
{
    ChannelReader<IEvent> Reader { get; }
    ChannelWriter<IEvent> Writer { get; }
}

internal sealed class MessageChannel : IMessageChannel
{
    private readonly Channel<IEvent> _messagesChannel = Channel.CreateUnbounded<IEvent>();
    public ChannelReader<IEvent> Reader => _messagesChannel.Reader;
    public ChannelWriter<IEvent> Writer => _messagesChannel.Writer;
}