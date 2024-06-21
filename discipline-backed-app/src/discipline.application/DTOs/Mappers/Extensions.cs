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

    internal static ActivityDto AsDto(this Activity activity)
        => new ActivityDto()
        {
            Id = activity.Id,
            Title = activity.Title,
            IsChecked = activity.IsChecked,
            ParentRuleId = activity.ParentRuleId
        };
}