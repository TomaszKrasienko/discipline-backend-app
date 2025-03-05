using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain;

public sealed class Stage : Entity<StageId, Ulid>
{
    public Title Title { get; private set; }
    public OrderIndex Index { get; set; }

    /// <summary>
    /// <remarks>Use only for Mongo purposes</remarks>
    /// </summary>
    public Stage(StageId stageId, Title title, OrderIndex index) : base(stageId)
    {
        Title = title;
        Index = index;
    }
    
    internal static Stage Create(StageId stageId, string title, int index)
        => new (stageId, title, index);
}