using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class InvalidCvvLengthException(string value)
    : DisciplineException($"Value: {value} is invalid cvv number");