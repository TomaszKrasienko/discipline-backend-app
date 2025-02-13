using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.application.DailyTrackers.DTOs.Responses;
using discipline.centre.dailytrackers.domain;
using discipline.centre.shared.abstractions.CQRS.Queries;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Queries;

/// <summary>
/// Gets activity by identifier.
/// </summary>
/// <param name="UserId">User identifier to query by.</param>
/// <param name="ActivityId"><see cref="Activity"/> identifier to query by</param>
public sealed record GetActivityByIdQuery(UserId UserId, ActivityId ActivityId) : IQuery<ActivityDto?>;
