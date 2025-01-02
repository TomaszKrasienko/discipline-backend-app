using discipline.centre.dailytrackers.domain.Specifications;

namespace discipline.centre.dailytrackers.application.DailyTrackers.DTOs;

public sealed record CreateActivityDto(DateOnly Day, ActivityDetailsSpecification Details,
    List<StageSpecification>? Stages);