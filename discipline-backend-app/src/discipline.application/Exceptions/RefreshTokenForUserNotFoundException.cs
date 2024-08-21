using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public sealed class RefreshTokenForUserNotFoundException()
    : DisciplineException("User does not exists for provided refresh token");