using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddAuth(configuration)
            .AddExceptionsHandling()
            .AddDal(configuration)
            .AddEvents(configuration)
            .AddIdentityContext()
            .AddTime();
    
    internal static IOptions<T> GetOptions<T>(this IServiceCollection services) where T : class
    {
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IOptions<T>>();
    }

}