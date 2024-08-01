using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class EmptyFeatureValueException()
    : DisciplineException("Feature can not be empty");