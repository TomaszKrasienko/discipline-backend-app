using Microsoft.AspNetCore.Builder;

namespace discipline.infrastructure.Exceptions.Configuration;

internal static class ExceptionsAppConfigurationExtensions
{
    internal static WebApplication UseExceptions(this WebApplication app)
    {
        app.UseExceptionHandler();
        return app;
    }
}