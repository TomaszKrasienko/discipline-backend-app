using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

internal static class ActivityRuleDocumentMappingExtensions
{
    internal static ActivityRule MapAsEntity(this ActivityRuleDocument document)
        => new(
            ActivityRuleId.Parse(document.Id),
            UserId.Parse(document.UserId),
            Details.Create(document.Title, document.Note), 
            document.Mode,
            document.SelectedDays is not null ? SelectedDays.Create(document.SelectedDays.ToList()) : null,
            document.Stages?.Select(x => x.MapAsEntity()).ToList()); 
    
    internal static ActivityRuleDto MapAsDto(this ActivityRuleDocument document)
        => new()
        {
            ActivityRuleIdId = ActivityRuleId.Parse(document.Id),
            Title = document.Title,
            Note = document.Note,
            Mode = document.Mode,
            SelectedDays = document.SelectedDays?.ToList(),
            Stages = document.Stages?.Select(x => x.MapAsDto()).ToList()
        };
}