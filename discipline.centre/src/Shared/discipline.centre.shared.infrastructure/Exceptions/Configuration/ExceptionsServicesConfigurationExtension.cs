using discipline.centre.shared.infrastructure.Exceptions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class ExceptionsServicesConfigurationExtension
{
    internal static IServiceCollection AddExceptionsHandling(this IServiceCollection services)
        => services
            .AddProblemDetails()
            .AddExceptionHandler<ExceptionHandler>();
}