using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.users.infrastructure.Configurations;

public static class UsersServicesInfrastructureConfigExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string assemblyName)
        => services
            .AddPasswordsSecure()
            .AddDal(assemblyName);
}