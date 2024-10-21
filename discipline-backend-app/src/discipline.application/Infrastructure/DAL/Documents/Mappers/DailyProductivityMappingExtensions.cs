using discipline.application.DTOs;
using discipline.domain.DailyProductivities.Entities;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.application.Infrastructure.DAL.Documents.Mappers;

internal static class DailyProductivityMappingExtensions
{
    internal static DailyProductivityDocument AsDocument(this DailyProductivity entity)
        => new()
        {
            Id = entity.Id.ToString(),
            Day = entity.Day,
            UserId = entity.UserId.Value,
            Activities = entity.Activities?.Select(x => x.AsDocument())
        };

    internal static DailyProductivity AsEntity(this DailyProductivityDocument document)
        => new(
            new(Ulid.Parse(document.Id)),
            document.Day, 
            new(document.UserId),
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
            Id = entity.Id.ToString(),
            IsChecked = entity.IsChecked,
            Title = entity.Title,
            ParentRuleId = entity.ParentRuleId?.ToString()
        };

    internal static Activity AsEntity(this ActivityDocument document)
        => new Activity(
            new(Ulid.Parse(document.Id)),
            document.Title,
            document.IsChecked,
            document.ParentRuleId is null ? null : new ActivityRuleId(Ulid.Parse(document.ParentRuleId)));

    internal static ActivityDto AsDto(this ActivityDocument document)
        => new()
        {
            Id = Ulid.Parse(document.Id),
            Title = document.Title,
            IsChecked = document.IsChecked,
            ParentRuleId = document.ParentRuleId is not null ? Ulid.Parse(document.ParentRuleId) : null
        };
}