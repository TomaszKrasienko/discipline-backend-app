using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.Exceptions;

public sealed class ActivityTitleAlreadyRegisteredException(string value, DateOnly day)
    : DisciplineException($"Activity title: {value} already registered for day:{day}");
