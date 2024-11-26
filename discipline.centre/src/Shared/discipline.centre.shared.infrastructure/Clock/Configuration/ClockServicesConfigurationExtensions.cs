using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.infrastructure.Clock;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class ClockServicesConfigurationExtensions
{
    internal static IServiceCollection AddClock(this IServiceCollection services)
        => services.AddSingleton<IClock, Clock>();
}