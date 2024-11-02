namespace discipline.centre.shared.abstractions.SharedKernel;

public interface IEntity
{
    public int Version { get; }
    void IncreaseVersion();
}