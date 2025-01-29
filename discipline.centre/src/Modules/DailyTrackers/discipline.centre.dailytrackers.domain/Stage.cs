using discipline.centre.dailytrackers.domain.ValueObjects;
using discipline.centre.dailytrackers.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain;

/// <summary>
/// Represents stage of an <see cref="Activity"/>
/// </summary>
public sealed class Stage : Entity<StageId, Ulid>
{
    /// <summary>
    /// Title of the stage.
    /// </summary>
    public Title Title { get; private set; }
    
    /// <summary>
    /// Order index of the stage, determining its position.
    /// </summary>
    public OrderIndex Index { get; private set; }
    
    /// <summary>
    /// Indicates whether stage is marked as checked.
    /// </summary>
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
    
    /// <summary>
    /// Creates a new instance of <see cref="Stage"/>.
    /// </summary>
    /// <param name="stageId">Unique identifier of the stage.</param>
    /// <param name="title">Title of the stage.</param>
    /// <param name="index">Order index of the stage.</param>
    /// <returns>New instance of <see cref="Stage"/></returns>
    internal static Stage Create(StageId stageId, string title, int index)
        => new (stageId, title, index, false);
    
    /// <summary>
    /// Marks stage as checked.
    /// </summary>
    internal void MarkAsChecked()
        => IsChecked = true;
    
    /// <summary>
    /// Updates order index of the stage.
    /// </summary>
    /// <param name="index">New index value</param>
    internal void ChangeIndex(int index)
        => Index = index;
}