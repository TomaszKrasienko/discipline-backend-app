using discipline.centre.users.infrastructure.Users.RefreshToken.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class UsersServicesInfrastructureConfigExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string assemblyName,
        IConfiguration configuration)
        => services
            .AddPasswordsSecure()
            .AddDal(assemblyName)
            .AddUsersAuth(configuration)
            .AddTokenStorage()
            .AddRefreshTokenStorage(configuration);
}