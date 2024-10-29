namespace discipline.application.Behaviours.CQRS.Commands;

internal interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}