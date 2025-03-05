using discipline.centre.shared.abstractions.Modules;
using discipline.centre.shared.infrastructure.Events.Brokers.Internal.Channels;
using Microsoft.Extensions.Hosting;

namespace discipline.centre.shared.infrastructure.Events.Brokers.Internal;

internal sealed class BackgroundMessageDispatcher(IMessageChannel messageChannel,
    IModuleClient moduleClient) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in messageChannel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await moduleClient.PublishAsync(message);
            }
            catch (Exception)
            {
                //TODO: Logger
            }
        }
    }
}