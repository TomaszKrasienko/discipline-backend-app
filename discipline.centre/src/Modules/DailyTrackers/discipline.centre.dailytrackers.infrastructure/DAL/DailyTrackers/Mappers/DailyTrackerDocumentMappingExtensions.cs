using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.ValueObjects.Activities;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

/// <summary>
/// Extensions that map Daily Tracker Document on related types
/// </summary>
internal static class DailyTrackerDocumentMappingExtensions
{
    /// <summary>
    /// Maps <see cref="DailyTrackerDocument"/> on <see cref="DailyTracker"/>
    /// </summary>
    /// <param name="document">Instance of <see cref="DailyTrackerDocument"/> to be mapped</param>
    /// <returns>Instance of <see cref="DailyTracker"/></returns>
    internal static DailyTracker MapAsDocument(this DailyTrackerDocument document)
        => new DailyTracker(
            DailyTrackerId.Parse(document.DailyTrackerId),
            document.Day,
            UserId.Parse(document.UserId),
            document.Activities.Select(MapAsEntity).ToList());

    private static Activity MapAsEntity(this ActivityDocument document)
        => new Activity(
            ActivityId.Parse(document.ActivityId),
            Details.Create(document.Title, document.Note),
            document.IsChecked,
            document.ParentActivityRuleId is not null ? ActivityRuleId.Parse(document.ParentActivityRuleId) : null,
            document.Stages?.Select(MapAsEntity).ToList());

    private static Stage MapAsEntity(this StageDocument document)
        => new (StageId.Parse(document.StageId), document.Title, document.Index, document.IsChecked);
}