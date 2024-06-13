using discipline.application.Configuration;
using discipline.application.Domain.Repositories;
using discipline.application.Infrastructure.DAL.Configuration.Options;
using discipline.application.Infrastructure.DAL.Repositories;
using discipline.application.Infrastructure.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Infrastructure.DAL.Configuration;

internal static class Extensions
{
    private const string SectionName = "Postgres";
    
    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddPostgreSqlDbContext(configuration)
            .AddServices()
            .AddInitializer(configuration);

    private static IServiceCollection AddPostgreSqlDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<PostgresOptions>(SectionName);
        services.AddDbContext<DisciplineDbContext>(x => x.UseNpgsql(options.ConnectionString));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IUnitOfWork, PostgresUnitOfWork>()
            .AddScoped<IActivityRuleRepository, PostgreSqlActivityRuleRepository>();

    private static IServiceCollection AddInitializer(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<PostgresOptions>(SectionName);
        if (options.WithMigration)
        {
            services.AddHostedService<DatabaseInitializer>();
        }

        return services;
    }
}