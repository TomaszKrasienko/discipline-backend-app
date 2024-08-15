using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class InvalidCardLengthException(string value)
    : DisciplineException($"Value: {value} is invalid card number");