using discipline.centre.shared.infrastructure.Configuration;
using discipline.centre.shared.infrastructure.Logging.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class LoggingWebApplicationBuilderConfigurationExtensions
{
    internal static WebApplicationBuilder UseLogging(this WebApplicationBuilder app)
    {
        var seqOptions = app.Services.GetOptions<SeqOptions>();
        var appOptions = app.Services.GetOptions<AppOptions>();
        
        app.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ConnectionName", appOptions.Name)
                .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message}{NewLine}")
                .WriteTo.Seq(seqOptions.Url);
        });

        return app;
    }
}