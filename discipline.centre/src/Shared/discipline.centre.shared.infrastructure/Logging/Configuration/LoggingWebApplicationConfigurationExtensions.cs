using discipline.centre.shared.infrastructure.Logging;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class LoggingWebApplicationConfigurationExtensions
{
    internal static IApplicationBuilder UseLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<UserContextEnrichmentMiddleware>();
        return app;
    }
}