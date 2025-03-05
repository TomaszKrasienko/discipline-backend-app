using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.abstractions.Exceptions;

public sealed class NotFoundException(string code, string resourceName, params string[] param)
    : DisciplineException(code, $"Resource {resourceName} with params: {string.Join(',', param)} not found")
{
}