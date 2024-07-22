using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class DefaultCreatedAtException()
    : DisciplineException("Crated at value can not be default date time value");