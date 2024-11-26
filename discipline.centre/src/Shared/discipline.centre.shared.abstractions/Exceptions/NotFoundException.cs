using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.abstractions.Exceptions;

public sealed class NotFoundException(string code, string resourceName, string param)
    : DisciplineException(code, $"Resource {resourceName} with param: {param} not found")
{
}