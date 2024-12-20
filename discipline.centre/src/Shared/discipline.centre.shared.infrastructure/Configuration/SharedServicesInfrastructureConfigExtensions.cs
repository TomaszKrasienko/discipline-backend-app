using System.Reflection;
using discipline.centre.shared.infrastructure.Configuration;
using discipline.centre.shared.infrastructure.Constraint.Configuration;
using discipline.centre.shared.infrastructure.Serialization.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class SharedServicesInfrastructureConfigExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies,
        IConfiguration configuration)
        => services
            .AddAppOptions(configuration)
            .AddUiDocumentation()
            .AddCqrs(assemblies)
            .AddDal(configuration)
            .AddEvents(configuration)
            .AddClock()
            .AddAuth(configuration)
            .AddSerializer()
            .AddDistributedCache(configuration)
            .AddExceptionsHandling()
            .AddValidation(assemblies)
            .AddIdentityContext()
            .AddConstraints();

    private static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration configuration)
        => services.ValidateAndBind<AppOptions, AppOptionsValidator>(configuration);
    
    private static IServiceCollection AddUiDocumentation(this IServiceCollection services)
        => services.AddSwaggerGen(swagger =>
        {
            var appOptions = services.GetOptions<AppOptions>();
            swagger.EnableAnnotations();
            swagger.CustomSchemaIds(x => x.FullName);
            swagger.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = appOptions.Name,
                Version = "v1"
            });
        });
    
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