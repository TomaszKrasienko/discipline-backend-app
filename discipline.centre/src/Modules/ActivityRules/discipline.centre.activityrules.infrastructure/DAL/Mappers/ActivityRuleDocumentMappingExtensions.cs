using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

internal static class ActivityRuleDocumentMappingExtensions
{
    internal static ActivityRule MapAsEntity(this ActivityRuleDocument document)
        => new(
            ActivityRuleId.Parse(document.Id),
            UserId.Parse(document.UserId),
            document.Title,
            document.Mode,
            document.SelectedDays?.Select(SelectedDay.Create).ToList());
    
    internal static ActivityRuleDto AsDto(this ActivityRuleDocument document)
        => new()
        {
            Id = Ulid.Parse(document.Id),
            Title = document.Title,
            Mode = document.Mode,
            SelectedDays = document.SelectedDays?.ToList()
        };
}