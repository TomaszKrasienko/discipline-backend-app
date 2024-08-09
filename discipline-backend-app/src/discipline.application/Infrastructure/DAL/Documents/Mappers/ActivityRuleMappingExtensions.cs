using discipline.application.DTOs;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.domain.SharedKernel;

namespace discipline.application.Infrastructure.DAL.Documents.Mappers;

internal static class ActivityRuleMappingExtensions
{
    internal static ActivityRuleDocument AsDocument(this ActivityRule entity)
        => new()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Title = entity.Title,
            Mode = entity.Mode,
            SelectedDays = entity.SelectedDays?.Select(x => x.Value).ToList()
        };

    internal static ActivityRule AsEntity(this ActivityRuleDocument document)
        => new(
            (EntityId)document.Id,
            (EntityId)document.UserId,
            (Title)document.Title,
            (Mode)document.Mode,
            document.SelectedDays?.Select(x => new SelectedDay(x)));

    internal static ActivityRuleDto AsDto(this ActivityRuleDocument document)
        => new()
        {
            Id = document.Id,
            Title = document.Title,
            Mode = document.Mode,
            SelectedDays = document.SelectedDays?.ToList()
        };
}