using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed class Feature : ValueObject
{
    public string Value { get; }

    public Feature(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyFeatureValueException();
        }

        Value = value;
    }

    public static implicit operator string(Feature feature)
        => feature.Value;

    public static implicit operator Feature(string value)
        => new Feature(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}