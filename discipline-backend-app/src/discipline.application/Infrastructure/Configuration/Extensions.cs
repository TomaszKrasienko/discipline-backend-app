using discipline.application.Infrastructure.DAL.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Infrastructure.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddDal(configuration);

}