using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Behaviours.CQRS.Queries;

namespace discipline.application.Behaviours.CQRS;

public interface ICqrsDispatcher
{
    Task HandleAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand;

    Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}