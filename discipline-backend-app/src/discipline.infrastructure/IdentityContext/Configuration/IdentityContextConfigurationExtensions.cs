using discipline.application.Behaviours.IdentityContext;
using discipline.infrastructure.IdentityContext;

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