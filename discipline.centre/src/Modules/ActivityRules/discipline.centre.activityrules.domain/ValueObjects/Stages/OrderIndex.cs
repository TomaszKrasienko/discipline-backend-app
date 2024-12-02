using discipline.centre.activityrules.domain.Rules.Stages;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.activityrules.domain.ValueObjects.Stages;

public sealed class OrderIndex : ValueObject
{
    private int _index;

    public int Value
    {
        get => _index;
        private init
        {
            CheckRule(new IndexCannotBeLessThan1Rule(value));
            _index = value;
        }
    }

    private OrderIndex(int value)
        => Value = value;

    public static OrderIndex Create(int value)
        => new (value);
    
    public static implicit operator int (OrderIndex index)
        => index.Value;
    
    public static implicit operator OrderIndex(int value)
        => Create(value);
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}