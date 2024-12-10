namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record StageId(Ulid Value) : ITypeId<StageId>
{
    public static StageId New()
        => new (Ulid.NewUlid());
    
    public static StageId Parse(string stringTypedId)
    { 
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(StageId)}");
        }

        return new StageId(parsedId);
    }

    public override string ToString()
        => Value.ToString();
}