using discipline.infrastructure.Exceptions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class ExceptionsHandlingConfigurationExtension
{
    internal static IServiceCollection AddExceptionsHandling(this IServiceCollection services)
        => services
            .AddProblemDetails()
            .AddExceptionHandler<ExceptionHandler>();
}