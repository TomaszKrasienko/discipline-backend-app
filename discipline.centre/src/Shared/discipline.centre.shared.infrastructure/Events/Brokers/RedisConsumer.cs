using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace discipline.centre.shared.infrastructure.Events.Brokers;

internal sealed class RedisConsumer<TCommand>(
    ILogger<RedisConsumer<TCommand>> logger,
    IConnectionMultiplexer connectionMultiplexer,
    ISerializer serializer,
    IServiceProvider serviceProvider) : BackgroundService where TCommand : class, ICommand
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = connectionMultiplexer.GetSubscriber();
        var channel = typeof(TCommand).Name;
        
        await subscriber.SubscribeAsync(new RedisChannel(channel, RedisChannel.PatternMode.Auto),
            async void (_, message) =>
            {
                try
                {
                    logger.LogInformation("Redis message of type: '{0}'", typeof(TCommand).Name);
                    var command = serializer.ToObject<TCommand>(message!);
                
                    if (command is null)
                    {
                        return;
                    }
                
                    var dispatcher = serviceProvider.GetRequiredService<ICqrsDispatcher>();
                    await dispatcher.HandleAsync(command, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
            });
    }
}