using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public sealed class EmptyRefreshTokenException()
    : DisciplineException("Refresh token can not be null or empty");