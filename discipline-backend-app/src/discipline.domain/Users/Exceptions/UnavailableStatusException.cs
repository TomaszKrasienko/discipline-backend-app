using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class UnavailableStatusException(string value)
    : DisciplineException($"Status: {value} is unavailable");