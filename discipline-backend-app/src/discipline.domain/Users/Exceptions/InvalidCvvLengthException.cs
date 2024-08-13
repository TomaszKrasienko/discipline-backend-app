using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class InvalidCvvLengthException(string value)
    : DisciplineException($"Value: {value} is invalid cvv number");