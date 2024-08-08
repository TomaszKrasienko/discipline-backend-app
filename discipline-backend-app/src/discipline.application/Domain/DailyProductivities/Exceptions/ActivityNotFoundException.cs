using discipline.application.Exceptions;
using discipline.domain.SharedKernel;

namespace discipline.application.Domain.DailyProductivities.Exceptions;

public sealed class ActivityNotFoundException(Guid activityId)
    : DisciplineException($"Activity with ID: {activityId} does not exists");