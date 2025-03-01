using discipline.centre.shared.abstractions.CQRS.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Events.Brokers.Redis.Configuration;

public static class RedisConsumerServiceRegistrationExtensions
{
    public static IServiceCollection AddRedisConsumerService<TCommand>(this IServiceCollection services)
        where TCommand : class, ICommand
        => services
            .AddHostedService<RedisConsumer<TCommand>>();
}