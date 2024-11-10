using discipline.centre.shared.abstractions.CQRS.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace discipline.centre.shared.infrastructure.Validation;

internal sealed class ValidationCommandHandlerDecorator<TCommand>(
    ILogger<ValidationCommandHandlerDecorator<TCommand>> logger,
    ICommandHandler<TCommand> handler,
    IServiceProvider serviceProvider) : ICommandHandler<TCommand> where TCommand : ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var validator = serviceProvider.GetService<IValidator<TCommand>>();
        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException($"{command.GetType().Name}.Validation",
                    "There was an error while validation",
                    validationResult.ToDictionary());
            }
        }
        else
        {
            logger.LogDebug("Command of type: {0} has not a validator", command.GetType());
        }

        await handler.HandleAsync(command, cancellationToken);
    }
}