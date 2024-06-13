using discipline.application.Features.Base.Abstractions;

namespace discipline.application.Features.Configuration.Base.Abstractions;

public interface ICommandDispatcher
{
    Task HandleAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand;

}