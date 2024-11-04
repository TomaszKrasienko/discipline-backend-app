using discipline.centre.users.infrastructure.DAL.Users.Repositories;
using discipline.centre.users.domain.Users.Repositories;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, string assemblyName)
        => services
            .AddMongoContext(assemblyName)
            .AddScoped<IReadUserRepository, MongoUserRepository>()
            .AddScoped<IWriteUserRepository, MongoUserRepository>();
}