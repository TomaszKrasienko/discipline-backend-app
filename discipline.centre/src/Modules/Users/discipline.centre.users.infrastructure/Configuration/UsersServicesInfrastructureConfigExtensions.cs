using discipline.centre.users.infrastructure.Events.Configuration;
using discipline.centre.users.infrastructure.Users.RefreshToken.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class UsersServicesInfrastructureConfigExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string assemblyName)
        => services
            .AddPasswordsSecure()
            .AddDal(assemblyName)
            .AddEvents()
            .AddUsersAuth()
            .AddTokenStorage()
            .AddRefreshTokenStorage();
}