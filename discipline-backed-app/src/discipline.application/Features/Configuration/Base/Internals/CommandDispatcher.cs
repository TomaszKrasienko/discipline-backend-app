using discipline.application.Features.Configuration.Base.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Features.Configuration.Base.Internals;

public class CommandDispatcher(
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