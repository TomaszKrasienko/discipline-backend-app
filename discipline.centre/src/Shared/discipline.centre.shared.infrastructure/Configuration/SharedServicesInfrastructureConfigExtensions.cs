using System.Reflection;
using discipline.centre.shared.infrastructure.Serialization.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class SharedServicesInfrastructureConfigExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies,
        IConfiguration configuration)
        => services
            .AddCqrs(assemblies)
            .AddDal(configuration)
            .AddEvents(configuration)
            .AddClock()
            .AddAuth(configuration)
            .AddSerializer()
            .AddDistributedCache(configuration)
            .AddExceptionsHandling()
            .AddValidation(assemblies);

    internal static IServiceCollection ValidateAndBind<TOptions, TOptionsValidator>(this IServiceCollection services,
        IConfiguration configuration) where TOptions : class where TOptionsValidator : IValidateOptions<TOptions>
    {
        services
            .AddOptions<TOptions>()
            .Bind(configuration.GetSection(typeof(TOptions).Name))
            .ValidateOnStart();
        return services;
    }

    internal static TOptions GetOptions<TOptions>(this IServiceCollection services) where TOptions : class
    {
        var sp = services.BuildServiceProvider();
        return sp.GetRequiredService<IOptions<TOptions>>().Value;
    }
}