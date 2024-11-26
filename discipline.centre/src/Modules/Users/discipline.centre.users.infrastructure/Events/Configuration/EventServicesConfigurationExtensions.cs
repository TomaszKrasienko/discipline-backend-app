using discipline.centre.shared.abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.users.infrastructure.Events.Configuration;

internal static class EventServicesConfigurationExtensions
{
    internal static IServiceCollection AddEvents(this IServiceCollection services)
        => services.AddTransient<IEventMapper, UsersEventMapper>();
}