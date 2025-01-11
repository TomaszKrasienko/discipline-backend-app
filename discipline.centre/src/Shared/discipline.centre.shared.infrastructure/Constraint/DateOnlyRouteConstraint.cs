using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace discipline.centre.shared.infrastructure.Constraint;

internal sealed class DateOnlyRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value) && value is string valueAsString)
        {
            return DateOnly.TryParseExact(valueAsString, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _);
        }
        return false;
    }
}