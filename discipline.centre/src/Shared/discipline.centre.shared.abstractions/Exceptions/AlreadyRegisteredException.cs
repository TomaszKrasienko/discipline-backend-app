namespace discipline.centre.shared.abstractions.Exceptions;

public sealed class AlreadyRegisteredException(string code, string message)
    : Exception(message)
{
    public string Code => code;
}