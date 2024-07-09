using discipline.application.Domain.ActivityRules;
using discipline.application.Domain.ActivityRules.Entities;
using discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.application.Domain.DailyProductivities.Entities;
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
        => new()
        {
            Id = entity.Id,
            IsChecked = entity.IsChecked,
            Title = entity.Title,
            ParentRuleId = entity.ParentRuleId
        };

    internal static Activity AsEntity(this ActivityDocument document)
        => new Activity(
            document.Id,
            document.Title,
            document.IsChecked,
            document.ParentRuleId);

    internal static ActivityDto AsDto(this ActivityDocument document)
        => new()
        {
            Id = document.Id,
            Title = document.Title,
            IsChecked = document.IsChecked,
            ParentRuleId = document.ParentRuleId
        };

    internal static DailyProductivityDocument AsDocument(this DailyProductivity entity)
        => new()
        {
            Day = entity.Day,
            Activities = entity.Activities?.Select(x => x.AsDocument())
        };

    internal static DailyProductivity AsEntity(this DailyProductivityDocument document)
        => new(
            document.Day, 
            document.Activities?.Select(x => x.AsEntity()).ToList());

    internal static DailyProductivityDto AsDto(this DailyProductivityDocument document)
        => new()
        {
            Day = document.Day,
            Activities = document.Activities?.Select(x => x.AsDto()).ToList()
        };
    
}