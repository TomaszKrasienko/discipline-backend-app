using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.infrastructure.DAL;
using discipline.centre.activityrules.infrastructure.DAL.Repositories;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, string assemblyName)
        => services
            .AddScoped<ActivityRulesMongoContext>()
            .AddScoped<IReadActivityRuleRepository, MongoActivityRuleRepository>()
            .AddScoped<IReadWriteActivityRuleRepository, MongoActivityRuleRepository>();
}