using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class EmptyFeaturesListException()
    : DisciplineException("Features can not be empty");