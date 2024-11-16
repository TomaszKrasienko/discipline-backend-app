using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using discipline.centre.shared.infrastructure.IdentityContext.Factories;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class IdentityContextConfigurationExtensions
{
    internal static IServiceCollection AddIdentityContext(this IServiceCollection services)
        => services
            .AddHttpContextAccessor()
            .AddSingleton<IIdentityContextFactory, IdentityContextFactory>()
            .AddScoped<IIdentityContext>(sp =>
            {
                var factory = sp.GetRequiredService<IIdentityContextFactory>();
                return factory.Create();
            });
}