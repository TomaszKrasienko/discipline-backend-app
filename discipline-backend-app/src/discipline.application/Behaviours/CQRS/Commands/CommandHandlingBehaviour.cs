using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours.CQRS.Commands;

internal static class CommandHandlingBehaviour
{
    internal static IServiceCollection AddCommandHandlingBehaviour(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        services.AddSingleton<ICqrsDispatcher, CqrsDispatcher>();
        return services;
    }
}