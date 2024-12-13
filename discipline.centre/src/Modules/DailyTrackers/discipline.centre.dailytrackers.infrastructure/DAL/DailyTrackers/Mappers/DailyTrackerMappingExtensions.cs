using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.dailytrackers.domain;

internal static class DailyTrackerMappingExtensions
{
    internal static DailyTrackerDocument MapAsDocument(this DailyTracker dailyTracker)
        => new()
        {
            DailyTrackerId = dailyTracker.Id.ToString(),
            UserId = dailyTracker.UserId.ToString(),
            Activities = dailyTracker.Activities.Select(MapAsDocument).ToList()
        };
    
    private static ActivityDocument MapAsDocument(this Activity activity)
        => new()
        {
            ActivityId = activity.Id.ToString(),
            Title = activity.Details.Title,
            Note = activity.Details.Note,
            IsChecked = activity.IsChecked,
            ParentActivityRuleId = activity.ParentActivityRuleId?.ToString(),
            Stages = activity.Stages?.Select(MapAsDocument).ToList()
        };
    
    private static StageDocument MapAsDocument(this Stage stage)
        => new ()
        {
            StageId = stage.Id.ToString(),
            Title = stage.Title,
            Index = stage.Index,
            IsChecked = stage.IsChecked
        };
}