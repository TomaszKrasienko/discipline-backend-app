using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class EmptyFeaturesListException()
    : DisciplineException("Features can not be empty");