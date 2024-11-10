using discipline.centre.activityrules.domain.Rules;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.activityrules.domain.ValueObjects;

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

    private Title(string value) => Value = value;

    public static Title Create(string value) => new Title(value);

    public static implicit operator string(Title title) => title.Value;

    public static implicit operator Title(string value) => Create(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}