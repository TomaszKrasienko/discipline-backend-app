using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

public sealed class ActivityTitleAlreadyRegisteredException(string value, DateTime day)
    : DisciplineException($"Activity title: {value} already registered for day:{day.Date}");
