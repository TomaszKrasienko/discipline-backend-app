using discipline.application.Behaviours.Time;
using discipline.infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class TimeConfigurationExtensions
{
    internal static IServiceCollection AddTime(this IServiceCollection services)
        => services.AddSingleton<IClock, Clock>();
}