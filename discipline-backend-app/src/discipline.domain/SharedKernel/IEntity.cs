namespace discipline.domain.SharedKernel;

public interface IEntity
{
    public int Version { get; }
    void IncreaseVersion();
}