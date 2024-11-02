namespace discipline.centre.shared.abstractions.SharedKernel.Exceptions;

public sealed class DomainException(
    string code, string message) : Exception(message)
{
    public string Code => code;
}