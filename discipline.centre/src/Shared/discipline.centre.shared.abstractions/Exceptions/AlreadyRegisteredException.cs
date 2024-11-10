using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.abstractions.Exceptions;

public sealed class AlreadyRegisteredException(string code, string message)
    : DisciplineException(code, message);