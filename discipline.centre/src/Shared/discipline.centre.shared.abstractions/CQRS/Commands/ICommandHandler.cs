using OneOf;

namespace discipline.centre.shared.abstractions.CQRS.Commands;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}