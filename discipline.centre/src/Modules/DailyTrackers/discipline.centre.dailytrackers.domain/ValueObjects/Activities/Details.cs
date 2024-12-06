using discipline.centre.dailytrackers.domain.Rules.Activities;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.dailytrackers.domain.ValueObjects.Activities;

public sealed class Details : ValueObject
{
    private readonly string _title = null!;

    public string Title
    {
        get => _title;
        private init
        {
            CheckRule(new DetailsTitleCannotBeEmptyRule(value));
            CheckRule(new DetailsTitleCannotBeLongerThan30Rule(value));
            _title = value.Trim();
        }
    }
    public string? Note { get; }

    private Details(string title, string? note)
    {
        Title = title;
        Note = note;
    }

    public static Details Create(string title, string? note)
        => new Details(title, note);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Title; 
        yield return Note;
    }
}