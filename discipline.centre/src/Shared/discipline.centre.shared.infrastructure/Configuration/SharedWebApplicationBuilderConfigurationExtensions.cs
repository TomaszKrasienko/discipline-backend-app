// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class SharedWebApplicationBuilderConfigurationExtensions
{
    public static WebApplicationBuilder UseInfrastructure(this WebApplicationBuilder app)
        => app.UseLogging();
}