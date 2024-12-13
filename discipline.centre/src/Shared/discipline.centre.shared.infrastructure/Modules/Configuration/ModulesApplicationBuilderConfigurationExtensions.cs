using discipline.centre.shared.abstractions.Modules;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extensions for configuring module requests in the application builder
/// </summary>
public static class ModulesApplicationBuilderConfigurationExtensions
{
    /// <summary>
    /// Enables the use of module request by retrieving the required <see cref="IModuleSubscriber"/> service.
    /// </summary>
    /// <param name="app">The application builder used to configure the app.</param>
    /// <returns>An instance of <see cref="IModuleSubscriber"/> for handling module requests.</returns>
    public static IModuleSubscriber UseModuleRequest(this IApplicationBuilder app)
        => app.ApplicationServices
            .GetRequiredService<IModuleSubscriber>();
}