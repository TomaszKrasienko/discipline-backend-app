using discipline.application.Behaviours.CQRS.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.infrastructure.Validation.Configuration;

internal static class ValidationServicesConfiguration
{
    internal static IServiceCollection AddValidation(this IServiceCollection services)
        => services
            .AddValidators()
            .AddDecorators();

    private static IServiceCollection AddDecorators(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        return services;
    }
    
    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }
}