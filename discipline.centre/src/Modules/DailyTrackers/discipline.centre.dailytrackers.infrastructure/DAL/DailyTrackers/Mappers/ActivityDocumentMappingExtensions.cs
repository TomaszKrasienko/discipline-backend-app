using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

internal static class ActivityDocumentMappingExtensions
{
    /// <summary>
    /// Maps <see cref="ActivityDocument"/> on <see cref="ActivityDto"/>
    /// </summary>
    /// <param name="document">Instance of <see cref="ActivityDocument"/> to be mapped</param>
    /// <returns>Mapped instance of <see cref="ActivityDto"/></returns>
    internal static ActivityDto MapAsDto(this ActivityDocument document)
        => new()
        {
            ActivityId = ActivityId.Parse(document.ActivityId),
            Details = new ActivityDetailsSpecification(document.Title, document.Note),
            IsChecked = document.IsChecked,
            ParentActivityRuleId = document.ParentActivityRuleId is not null
                ? ActivityRuleId.Parse(document.ParentActivityRuleId)
                : null,
            Stages = document.Stages?.Select(x => x.MapAsDto()).ToList()
        };

    private static StageDto MapAsDto(this StageDocument document)
        => new()
        {
            StageId = StageId.Parse(document.StageId),
            Title = document.Title,
            Index = document.Index,
            IsChecked = document.IsChecked,
        };
}