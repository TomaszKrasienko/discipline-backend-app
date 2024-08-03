using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class UnavailableStatusException(string value)
    : DisciplineException($"Status: {value} is unavailable");