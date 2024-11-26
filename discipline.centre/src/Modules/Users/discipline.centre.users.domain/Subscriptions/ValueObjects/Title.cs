using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Subscriptions.Rules;

namespace discipline.centre.users.domain.Subscriptions.ValueObjects;

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

    public static Title Create(string value) => new (value);
    
    private Title(string value) => Value = value;

    public static implicit operator string(Title title) => title.Value;

    public static implicit operator Title(string value) => Create(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}