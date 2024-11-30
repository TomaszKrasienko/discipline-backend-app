namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record StageId(int Value) : ITypeId<StageId>
{
    public static StageId Parse(string stringTypedId)
        => new StageId(int.Parse(stringTypedId));
}