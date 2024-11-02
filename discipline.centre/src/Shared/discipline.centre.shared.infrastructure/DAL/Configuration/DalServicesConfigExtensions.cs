using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.DAL.Configuration;

internal static class DalServicesConfigExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services;

    private static IServiceCollection ValidateAndAddOptions(this IServiceCollection services,
        IConfiguration configuration)
        => services.BindAndValidate<MongoDbOptions, MongoDbOptionsValidator>(
            configuration.GetSection(configuration.GetSection(nameof(MongoDbOptions))));

}