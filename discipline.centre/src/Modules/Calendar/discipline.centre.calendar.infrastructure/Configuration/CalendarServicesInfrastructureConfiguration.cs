// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CalendarServicesInfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddDal();
}