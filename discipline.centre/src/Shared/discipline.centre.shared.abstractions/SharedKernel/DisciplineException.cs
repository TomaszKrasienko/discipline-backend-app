namespace discipline.centre.shared.abstractions.SharedKernel;

public abstract class DisciplineException(string code, string message)
    : Exception(message)
{
    public string Code => code;
}