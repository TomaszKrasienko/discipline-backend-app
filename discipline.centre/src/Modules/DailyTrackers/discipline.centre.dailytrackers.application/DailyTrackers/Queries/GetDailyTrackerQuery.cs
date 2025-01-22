using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.shared.abstractions.CQRS.Queries;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Queries;

public sealed record GetDailyTrackerQuery(UserId UserId, DateOnly Day) : IQuery<DailyTrackerDto?>;