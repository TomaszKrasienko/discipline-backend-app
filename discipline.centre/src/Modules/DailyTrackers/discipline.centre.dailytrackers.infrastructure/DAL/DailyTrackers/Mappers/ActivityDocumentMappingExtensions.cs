using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.domain.ValueObjects.Activities;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

internal static class ActivityDocumentMappingExtensions
{
    /// <summary>
    /// Maps <see cref="ActivityDocument"/> as <see cref="ActivityDto"/>
    /// </summary>
    /// <param name="document">Instance of <see cref="ActivityDocument"/> to be mapped</param>
    /// <returns>Mapped instance of <see cref="ActivityDto"/></returns>
    internal static ActivityDto AsDto(this ActivityDocument document)
        => new()
        {
            ActivityId = ActivityId.Parse(document.ActivityId),
            Details = new ActivityDetailsSpecification(document.Title, document.Note),
            IsChecked = document.IsChecked,
            ParentActivityRuleId = document.ParentActivityRuleId is not null
                ? ActivityRuleId.Parse(document.ParentActivityRuleId)
                : null,
            Stages = document.Stages?.Select(x => x.AsDto()).ToList()
        };

    private static StageDto AsDto(this StageDocument document)
        => new()
        {
            StageId = StageId.Parse(document.StageId),
            Title = document.Title,
            Index = document.Index,
            IsChecked = document.IsChecked,
        };
    
    /// <summary>
    /// Maps <see cref="ActivityDocument"/> as <see cref="Activity"/>
    /// </summary>
    /// <param name="document">Instance of <see cref="ActivityDocument"/> to be mapped</param>
    /// <returns>Mapped instance of <see cref="Activity"/></returns>
    internal static Activity AsEntity(this ActivityDocument document)
        => new (
            ActivityId.Parse(document.ActivityId),
            Details.Create(document.Title, document.Note),
            document.IsChecked,
            document.ParentActivityRuleId is not null ? ActivityRuleId.Parse(document.ParentActivityRuleId) : null,
            document.Stages?.Select(AsEntity).ToHashSet());

    private static Stage AsEntity(this StageDocument document)
        => new (StageId.Parse(document.StageId), document.Title, document.Index, document.IsChecked);
}