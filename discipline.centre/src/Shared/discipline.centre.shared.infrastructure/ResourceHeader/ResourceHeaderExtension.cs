using Microsoft.AspNetCore.Http;

namespace discipline.centre.shared.infrastructure.ResourceHeader;

public static class ResourceHeaderExtension
{
    internal const string HeaderName = "x-resource-id";
    
    public static void AddResourceIdHeader(this IHttpContextAccessor httpContext, string id)
        =>  httpContext.HttpContext?.Response.Headers.TryAdd(HeaderName, id);
}