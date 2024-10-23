using discipline.domain.SharedKernel;
using discipline.domain.Users.BusinessRules.Titles;

namespace discipline.domain.Users.ValueObjects;

public sealed class Title : ValueObject
{
    private readonly string _value = null!;

    public string Value
    {
        get => _value;
        private init
        {
            CheckRule(new TitleCanNotBeEmptyRule(value));
            _value = value;
        }
    }

    private Title(string value)
        => Value = value;

    public static implicit operator string(Title title)
        => title.Value;

    public static implicit operator Title(string value)
        => new Title(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}