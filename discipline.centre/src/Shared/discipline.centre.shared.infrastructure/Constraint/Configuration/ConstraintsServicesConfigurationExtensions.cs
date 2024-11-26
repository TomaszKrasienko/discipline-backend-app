using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Constraint.Configuration;

internal static class ConstraintsServicesConfigurationExtensions
{
    internal static IServiceCollection AddConstraints(this IServiceCollection services)
        => services.AddRouting(options =>
        {
            options.ConstraintMap.Add("ulid", typeof(UlidRouteConstraint));
        });
}