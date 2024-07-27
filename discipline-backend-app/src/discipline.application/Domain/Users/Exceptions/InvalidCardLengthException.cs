using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class InvalidCardLengthException(string value)
    : DisciplineException($"Value: {value} is invalid card number");