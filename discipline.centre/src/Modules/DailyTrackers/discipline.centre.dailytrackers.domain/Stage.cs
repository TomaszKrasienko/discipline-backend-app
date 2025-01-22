using discipline.centre.dailytrackers.domain.ValueObjects;
using discipline.centre.dailytrackers.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain;

public sealed class Stage : Entity<StageId, Ulid>
{
    public Title Title { get; private set; }
    public OrderIndex Index { get; private set; }
    public IsChecked IsChecked { get; private set; }
    
    /// <summary>
    /// <remarks>Use only for Mongo purposes</remarks>
    /// </summary>
    public Stage(StageId stageId, Title title, OrderIndex index, IsChecked isChecked) : base(stageId)
    {
        Title = title;
        Index = index;
        IsChecked = isChecked;
    }
    
    internal static Stage Create(StageId stageId, string title, int index)
        => new (stageId, title, index, false);
    
    internal void MarkAsChecked()
        => IsChecked = true;
}