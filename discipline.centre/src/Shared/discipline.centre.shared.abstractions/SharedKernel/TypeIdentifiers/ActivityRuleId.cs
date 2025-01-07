namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record ActivityRuleId(Ulid Value) : ITypeId<ActivityRuleId, Ulid>
{
    public static ActivityRuleId Create() => Create(Ulid.NewUlid());
    
    public static ActivityRuleId Create(Ulid value) => new(value);

    public static ActivityRuleId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(ActivityRuleId)}");
        }

        return new ActivityRuleId(parsedId);
    }
}