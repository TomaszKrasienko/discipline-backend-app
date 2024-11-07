namespace discipline.centre.shared.abstractions.Exceptions;

public sealed class NotFoundException(string code, string resourceName, string param)
    : Exception($"Resource {resourceName} with param: {param} not found")
{
    public string Code => code;
}