using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class DefaultCreatedAtException()
    : DisciplineException("Crated at value can not be default date time value");