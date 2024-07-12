using discipline.application.Domain.DailyProductivities.Entities;
using discipline.application.DTOs;

namespace discipline.application.Infrastructure.DAL.Documents.Mappers;

internal static class DailyProductivityMappingExtensions
{
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
}