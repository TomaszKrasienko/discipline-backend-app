using discipline.application.Domain.ActivityRules.Entities;
using discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.application.DTOs;

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
            document.Id,
            document.UserId,
            document.Title,
            document.Mode,
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