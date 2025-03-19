using System.Reflection;
using discipline.centre.shared.abstractions.Attributes;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.CQRS.Queries;
using discipline.centre.shared.infrastructure.Cqrs;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;


internal static class CqrsServicesConfigExtensions
{
    internal static IServiceCollection AddCqrs(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c 
                => c.AssignableTo(typeof(ICommandHandler<>)).WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.AddSingleton<ICqrsDispatcher, CqrsDispatcher>();
        
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c 
                => c.AssignableTo(typeof(IQueryHandler<,>)).WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddSingleton<ICqrsDispatcher, CqrsDispatcher>();
        return services;
    }
}