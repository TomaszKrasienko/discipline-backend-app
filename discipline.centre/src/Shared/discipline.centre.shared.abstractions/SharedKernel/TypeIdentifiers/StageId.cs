namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record StageId(Ulid Value) : ITypeId<StageId>
{
    public static StageId New()
        => new StageId(new Ulid());
    
    public static StageId Parse(string stringTypedId)
        => new StageId(Ulid.Parse(stringTypedId));
}