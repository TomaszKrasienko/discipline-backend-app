using discipline.application.Infrastructure.DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours;

internal static class CreatingTransactionBehaviour
{
    internal static IServiceCollection AddCreatingTransaction(this IServiceCollection services)
    {
        //services.Decorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
        return services;
    }
}

internal sealed class UnitOfWorkCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> handler,
    IUnitOfWork unitOfWork) : ICommandHandler<TCommand> where TCommand : ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        => await unitOfWork.ExecuteAsync(async () => await handler.HandleAsync(command, cancellationToken),
            cancellationToken);
}


