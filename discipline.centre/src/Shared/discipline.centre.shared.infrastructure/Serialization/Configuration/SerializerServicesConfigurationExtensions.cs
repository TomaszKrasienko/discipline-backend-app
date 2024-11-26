using discipline.centre.shared.abstractions.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Serialization.Configuration;

internal static class SerializerServicesConfigurationExtensions
{
    internal static IServiceCollection AddSerializer(this IServiceCollection services)
        => services.AddSingleton<ISerializer, Serializer>();
}