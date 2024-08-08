using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public class ValidationException(Type command, List<string> errorMessages)
    : DisciplineException($"There was errors for type: {command} with details: {string.Join(",", errorMessages)}");