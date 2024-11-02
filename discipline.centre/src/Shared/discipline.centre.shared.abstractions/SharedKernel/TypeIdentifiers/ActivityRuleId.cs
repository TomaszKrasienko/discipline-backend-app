namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record ActivityRuleId(Ulid Value) : ITypeId<ActivityRuleId>
{
    public static ActivityRuleId New()
        => new(Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();
}