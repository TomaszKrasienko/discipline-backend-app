using discipline.centre.shared.infrastructure.Events.Brokers.Internal.Abstractions;
using discipline.centre.shared.infrastructure.Events.Brokers.Internal.Channels;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Events.Brokers.Internal.Configuration;

internal static class InternalBrokerServicesConfiguration
{
    internal static IServiceCollection AddInternalBrokerServices(this IServiceCollection services)
        => services 
            .AddSingleton<IMessageChannel, MessageChannel>()
            .AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>()
            .AddHostedService<BackgroundMessageDispatcher>();
}