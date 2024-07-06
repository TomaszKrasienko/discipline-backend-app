using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours;

internal static class CommandHandlingBehaviour
{
    internal static IServiceCollection AddCommandHandlingBehaviour(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        return services;
    }
}

//Marker
internal interface ICommand { }

internal interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

internal interface ICommandDispatcher
{
    Task HandleAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand;
}

internal class CommandDispatcher(
    IServiceProvider serviceProvider) : ICommandDispatcher
{
    public async Task HandleAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, cancellationToken);
    }
}