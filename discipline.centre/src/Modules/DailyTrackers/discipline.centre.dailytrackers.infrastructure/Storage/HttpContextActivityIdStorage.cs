using discipline.centre.dailytrackers.application.DailyTrackers.Services;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Http;

namespace discipline.centre.dailytrackers.infrastructure.Storage;

internal sealed class HttpContextActivityIdStorage(
    IHttpContextAccessor httpContextAccessor) : IActivityIdStorage
{
    private const string Key = "activity_id";

    public void Set(ActivityId activityId)
        => httpContextAccessor.HttpContext?.Items.Add(Key, activityId);

    public ActivityId? Get()
        => httpContextAccessor.HttpContext!
            .Items.TryGetValue(Key, out var result)
            ? (ActivityId?)result : null;
}