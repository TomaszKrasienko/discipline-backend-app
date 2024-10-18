using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.DailyProductivities.Exceptions;

public sealed class ActivityNotFoundException(ActivityId activityId)
    : DisciplineException($"Activity with ID: {activityId.ToString()} does not exists");