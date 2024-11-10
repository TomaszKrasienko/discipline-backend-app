using System.Reflection;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.infrastructure.Validation;
using FluentValidation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class ValidationServicesConfigurationExtensions
{
    internal static IServiceCollection AddValidation(this IServiceCollection services, IList<Assembly> assemblies)
        => services
            .AddValidators(assemblies)
            .AddDecorators();

    private static IServiceCollection AddDecorators(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        return services;
    }
    
    private static IServiceCollection AddValidators(this IServiceCollection services, IList<Assembly> assemblies)
    {
        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }
}