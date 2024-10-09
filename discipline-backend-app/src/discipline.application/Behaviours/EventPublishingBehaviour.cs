using discipline.application.Features.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace discipline.application.Behaviours;

internal static class EventPublishingBehaviour
{
    private const string SectionName = "Redis";
    
    internal static IServiceCollection AddEventPublishingBehaviour(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration)
            .AddSingleton(sp =>
            {
                var redisOptions = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
                return ConnectionMultiplexer.Connect(redisOptions.ConnectionString);
            })
            .AddScoped(sp =>
            {
                var connection = sp.GetRequiredService<ConnectionMultiplexer>();
                return connection.GetSubscriber();
            })
            .AddScoped<IEventPublisher, RedisEventPublisher>();
        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<RedisOptions>(configuration.GetSection(SectionName));
}

public sealed record RedisOptions
{
    public string ConnectionString { get; init; }
}

//Marker
public interface IEvent;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : class, IEvent;
}

internal sealed class RedisEventPublisher(
    ISubscriber subscriber) : IEventPublisher
{
    public async Task PublishAsync<T>(T @event) where T : class, IEvent
    {
        if (!EventsRoutes.Routes.TryGetValue(@event.GetType(), out var channel))
        {
            throw new ArgumentException("Event not registered");
        }
        await subscriber.PublishAsync( channel, @event.AsJson());
    }
}

internal static class EventsRoutes
{
    internal static readonly Dictionary<Type, string> Routes = new()
    {
        [typeof(UserSignedUp)] = "new-user"
    };
} 

