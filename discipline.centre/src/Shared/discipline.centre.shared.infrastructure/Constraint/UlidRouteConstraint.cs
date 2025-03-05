using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace discipline.centre.shared.infrastructure.Constraint;

internal sealed class UlidRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value) && value is string valueString)
        {
            return Ulid.TryParse(valueString, out _);
        }
        return false;
    }
}