using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.CQRS.Queries;

namespace discipline.centre.shared.abstractions.CQRS;

public interface ICqrsDispatcher
{
    Task HandleAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand;
    
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}