using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.shared.abstractions.CQRS.Queries;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Queries;

public sealed record GetActivityByIdQuery(UserId UserId, ActivityId ActivityId)
    : IQuery<ActivityDto?>;