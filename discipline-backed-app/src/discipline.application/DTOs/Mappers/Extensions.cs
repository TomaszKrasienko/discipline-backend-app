using discipline.application.Domain.Entities;

namespace discipline.application.DTOs.Mappers;

internal static class Extensions
{
    internal static ActivityRuleDto AsDto(this ActivityRule entity)
        => new ActivityRuleDto()
        {
            Id = entity.Id,
            Title = entity.Title,
            Mode = entity.Mode,
            SelectedDays = entity.SelectedDays?.Select(x => x.Value).ToList()
        };
}