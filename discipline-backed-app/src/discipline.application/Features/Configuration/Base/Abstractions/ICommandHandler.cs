using discipline.application.Features.Base.Abstractions;

namespace discipline.application.Features.Configuration.Base.Abstractions;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}