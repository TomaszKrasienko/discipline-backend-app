using discipline.application.Behaviours.CQRS.Commands;
using FluentValidation;
using ValidationException = discipline.application.Exceptions.ValidationException;

namespace discipline.infrastructure.Validation;

internal sealed class ValidationCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> handler,
    IValidator<TCommand> validator) : ICommandHandler<TCommand> where TCommand : ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(command.GetType(), validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        await handler.HandleAsync(command, cancellationToken);
    }
}