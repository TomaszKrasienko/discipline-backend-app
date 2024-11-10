// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class SharedWebApplicationInfrastructureConfigExtensions
{
    public static WebApplication UseInfrastructure(this WebApplication app)
        => app.UseExceptionsHandling();
}