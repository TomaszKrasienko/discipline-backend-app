using discipline.application.Domain.Entities;
using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.application.DTOs;

namespace discipline.application.Infrastructure.DAL.Documents.Mappers;

internal static class Extensions
{
    internal static ActivityRuleDocument AsDocument(this ActivityRule entity)
        => new()
        {
            Id = entity.Id,
            Title = entity.Title,
            Mode = entity.Mode,
            SelectedDays = entity.SelectedDays?.Select(x => x.Value).ToList()
        };

    internal static ActivityRule AsEntity(this ActivityRuleDocument document)
        => new(
            document.Id,
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

    internal static ActivityDocument AsDocument(this Activity entity)
        => new ()
        {
            Id = entity.Id,
            IsChecked = entity.IsChecked,
            Title = entity.Title,
            ParentRuleId = entity.ParentRuleId
        };
}