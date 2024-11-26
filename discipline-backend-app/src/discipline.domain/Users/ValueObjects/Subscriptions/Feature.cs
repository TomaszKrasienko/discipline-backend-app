using discipline.domain.SharedKernel;
using discipline.domain.Users.BusinessRules.Features;

namespace discipline.domain.Users.ValueObjects.Subscriptions;

public sealed class Feature : ValueObject
{
    private readonly string _value = null!;
    public string Value
    {
        get => _value;
        private init
        {
            CheckRule(new FeatureCanNotBeEmptyRule(value));
            _value = value;
        }
    }

    public static Feature Create(string value)
        => new(value);

    private Feature(string value)
        => Value = value;
    
    public static implicit operator string(Feature feature)
        => feature.Value;

    public static implicit operator Feature(string value)
        => new(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}