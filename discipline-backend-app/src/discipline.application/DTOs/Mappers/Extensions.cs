using discipline.application.Domain.ActivityRules;
using discipline.application.Domain.ActivityRules.Entities;
using discipline.application.Domain.DailyProductivities.Entities;

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

    internal static ActivityDto AsDto(this Activity entity)
        => new ActivityDto()
        {
            Id = entity.Id,
            Title = entity.Title,
            IsChecked = entity.IsChecked,
            ParentRuleId = entity.ParentRuleId
        };

    internal static DailyProductivityDto AsDto(this DailyProductivity entity)
        => new DailyProductivityDto()
        {
            Day = entity.Day,
            Activities = entity.Activities?.Select(x => x.AsDto()).ToList()
        };
}