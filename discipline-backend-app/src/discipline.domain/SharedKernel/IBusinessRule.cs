namespace discipline.domain.SharedKernel;

public interface IBusinessRule
{
    public Exception Exception { get; }
    bool IsBroken();
}