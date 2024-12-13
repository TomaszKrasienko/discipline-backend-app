using discipline.centre.dailytrackers.application.DailyTrackers.Services;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Http;

namespace discipline.centre.dailytrackers.infrastructure.Storage;

internal sealed class HttpContextActivityIdStorage(
    IHttpContextAccessor httpContextAccessor) : IActivityIdStorage
{
    private const string Key = "activity_id";
    
    public void Set(ActivityId activityId)
    {
        throw new NotImplementedException();
    }

    public ActivityId? Get()
    {
        throw new NotImplementedException();
    }
}