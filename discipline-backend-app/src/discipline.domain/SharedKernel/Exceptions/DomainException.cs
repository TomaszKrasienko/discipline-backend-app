namespace discipline.domain.SharedKernel.Exceptions;

public sealed class DomainException(
    string code, string message) : Exception
{
    public string Code => code;
}