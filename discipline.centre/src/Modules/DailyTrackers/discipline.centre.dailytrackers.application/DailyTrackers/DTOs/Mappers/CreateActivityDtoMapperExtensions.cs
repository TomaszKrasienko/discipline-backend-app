using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.dailytrackers.application.DailyTrackers.Commands;

// ReSharper disable once CheckNamespace
namespace discipline.centre.dailytrackers.application.DailyTrackers.DTOs;

public static class CreateActivityDtoMapperExtensions
{
    public static CreateActivityCommand MapAsCommand(this CreateActivityDto dto, UserId userId, ActivityId activityId)
        => new (userId, activityId, dto.Day, dto.Details, dto.Stages);
}