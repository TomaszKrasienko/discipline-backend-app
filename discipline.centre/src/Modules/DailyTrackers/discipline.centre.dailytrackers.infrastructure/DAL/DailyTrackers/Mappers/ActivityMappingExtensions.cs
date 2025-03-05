using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using ActivityDocument = discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents.ActivityDocument;

// ReSharper disable once CheckNamespace
namespace discipline.centre.dailytrackers.domain;

internal static class ActivityMappingExtensions
{
    /// <summary>
    /// Maps <see cref="Activity"/> as <see cref="ActivityDocument"/>
    /// </summary>
    /// <param name="activity">An instance of <see cref="Activity"/> to be mapped</param>
    /// <returns>Mapped <see cref="ActivityDocument"/></returns>
    internal static ActivityDocument AsDocument(this Activity activity)
        => new()
        {
            ActivityId = activity.Id.ToString(),
            Title = activity.Details.Title,
            Note = activity.Details.Note,
            IsChecked = activity.IsChecked,
            ParentActivityRuleId = activity.ParentActivityRuleId?.ToString(),
            Stages = activity.Stages?.Select(AsDocument).ToList()
        };
    
    private static StageDocument AsDocument(this Stage stage)
        => new ()
        {
            StageId = stage.Id.ToString(),
            Title = stage.Title,
            Index = stage.Index,
            IsChecked = stage.IsChecked
        };
}