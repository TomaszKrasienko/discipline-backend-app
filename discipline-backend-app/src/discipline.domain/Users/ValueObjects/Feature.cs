using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed record Feature
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
}