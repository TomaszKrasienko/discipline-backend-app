using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain;

public sealed class Stage : Entity<StageId>
{
    public Title Title { get; private set; }
    
    /// <summary>
    /// <remarks>Use only for Mongo purposes</remarks>
    /// </summary>
    public Stage(StageId stageId, Title title) : base(stageId)
        => Title = title;
    
    internal static Stage Create(StageId stageId, Title title)
        => new Stage(stageId, title);
}