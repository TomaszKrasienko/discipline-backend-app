using discipline.centre.shared.infrastructure.Events.Brokers;
using discipline.centre.shared.infrastructure.Events.Brokers.Redis;
using discipline.centre.shared.infrastructure.Events.Brokers.Redis.Abstractions;
using discipline.centre.shared.infrastructure.Events.Brokers.Redis.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class RedisBrokerServicesConfigurationExtensions
{
    internal static IServiceCollection AddRedisBroker(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddServices();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.ValidateAndBind<RedisBrokerOptions, RedisBrokerOptionsValidator>(configuration);

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RedisBrokerOptions>>();
                var connectionString = options.Value.ConnectionString;

                return ConnectionMultiplexer.Connect(connectionString);
            })
            .AddScoped<IRedisPubSubClient, RedisPubSubClient>();
}