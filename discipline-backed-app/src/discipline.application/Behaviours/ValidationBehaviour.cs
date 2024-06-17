using discipline.application.Features.Base.Abstractions;
using discipline.application.Features.Configuration.Base.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ValidationException = discipline.application.Exceptions.ValidationException;

namespace discipline.application.Behaviours;

internal static class ValidationBehaviour
{
    internal static IServiceCollection AddValidationBehaviour(this IServiceCollection services)
        => services
            .AddValidators()
            .AddDecorators();

    private static IServiceCollection AddDecorators(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        return services;
    }
    
    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }
}

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