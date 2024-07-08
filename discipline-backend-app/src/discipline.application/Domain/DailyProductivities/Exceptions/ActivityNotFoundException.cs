using discipline.application.Exceptions;

namespace discipline.application.Domain.DailyProductivities.Exceptions;

public sealed class ActivityNotFoundException(Guid activityId)
    : DisciplineException($"Activity with ID: {activityId} does not exists");