using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class EmptyFeatureValueException()
    : DisciplineException("Feature can not be empty");