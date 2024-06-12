using Microsoft.AspNetCore.Http;

namespace discipline.application.Behaviours;

internal static class AddingResourceIdHeader
{
    internal const string HeaderName = "x-resource-id";
    internal static void AddResourceIdHeader(this HttpContext httpContext, Guid id)
        =>  httpContext.Response.Headers.TryAdd(HeaderName, id.ToString());
}