using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.infrastructure.DAL;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, string assemblyName)
        => services
            .AddMongoContext(assemblyName)
            .AddScoped<IReadActivityRuleRepository, MongoActivityRuleRepository>()
            .AddScoped<IWriteActivityRuleRepository, MongoActivityRuleRepository>();
}