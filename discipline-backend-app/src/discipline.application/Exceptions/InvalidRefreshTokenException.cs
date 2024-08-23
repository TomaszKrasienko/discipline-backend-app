using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public sealed class InvalidRefreshTokenException() 
    : AuthorizeException("Refresh token is invalid");