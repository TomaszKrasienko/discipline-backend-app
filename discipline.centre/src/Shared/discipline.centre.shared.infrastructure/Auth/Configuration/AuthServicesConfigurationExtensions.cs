using discipline.centre.shared.infrastructure.Auth.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class AuthServicesConfigurationExtensions
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndAddOptions(configuration);

    private static IServiceCollection ValidateAndAddOptions(this IServiceCollection services,
        IConfiguration configuration)
        => services.ValidateAndBind<AuthOptions, AuthOptionsValidator>(configuration);
}