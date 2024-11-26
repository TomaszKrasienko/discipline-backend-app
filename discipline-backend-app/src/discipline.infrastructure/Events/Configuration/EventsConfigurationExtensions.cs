using discipline.application.Behaviours;
using discipline.application.Behaviours.Events;
using discipline.infrastructure.Events.Configuration;
using discipline.infrastructure.Events.Publisher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class EventsConfigurationExtensions
{
    internal static IServiceCollection AddEvents(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration);
        services.AddSingleton<IEventsChannelConventionProvider, EventsChannelConventionProvider>();
        services.AddScoped<IEventPublisher, RedisEventPublisher>();
        services.AddScoped<IEventProcessor, EventProcessor>();
        services.AddSingleton(sp =>
            {
                var redisOptions = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
                return ConnectionMultiplexer.Connect(redisOptions.ConnectionString);
            })
            .AddScoped(sp =>
            {
                var connection = sp.GetRequiredService<ConnectionMultiplexer>();
                return connection.GetSubscriber();
            });
        return services;
    }
    
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<RedisOptions>(configuration.GetSection(nameof(RedisOptions)));
}