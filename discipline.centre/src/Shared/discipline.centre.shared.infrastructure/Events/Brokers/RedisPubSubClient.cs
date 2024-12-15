using discipline.centre.shared.infrastructure.Events.Brokers.Abstractions;
using StackExchange.Redis;

namespace discipline.centre.shared.infrastructure.Events.Brokers;

internal sealed class RedisPubSubClient(
    IConnectionMultiplexer connectionMultiplexer) : IRedisPubSubClient
{
    private readonly ISubscriber _subscriber = connectionMultiplexer.GetSubscriber();
    
    public Task SendAsync(string json, string route, CancellationToken cancellationToken = default)
        => _subscriber.PublishAsync(new RedisChannel(route, RedisChannel.PatternMode.Auto), json);    
}