using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.Exceptions;

public sealed class ActivityNotFoundException(Guid activityId)
    : DisciplineException($"Activity with ID: {activityId} does not exists");