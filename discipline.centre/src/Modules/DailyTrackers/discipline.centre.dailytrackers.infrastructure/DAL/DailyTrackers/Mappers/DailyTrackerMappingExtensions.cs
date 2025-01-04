using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.dailytrackers.domain;

internal static class DailyTrackerMappingExtensions
{
    /// <summary>
    /// Maps <see cref="DailyTracker"/> as a <see cref="DailyTrackerDocument"/>
    /// </summary>
    /// <param name="dailyTracker">An instance of <see cref="Activity"/> to be mapped</param>
    /// <returns>Mapped <see cref="DailyTrackerDocument"/></returns>
    internal static DailyTrackerDocument MapAsDocument(this DailyTracker dailyTracker)
        => new()
        {
            DailyTrackerId = dailyTracker.Id.ToString(),
            UserId = dailyTracker.UserId.ToString(),
            Activities = dailyTracker.Activities.Select(x => x.MapAsDocument()).ToList()
        };
}