namespace discipline.centre.shared.abstractions.SharedKernel;

public interface IBusinessRule
{
    public Exception Exception { get; }
    bool IsBroken();
}