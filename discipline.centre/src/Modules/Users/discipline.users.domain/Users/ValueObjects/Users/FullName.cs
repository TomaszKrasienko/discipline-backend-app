using discipline.centre.shared.abstractions.SharedKernel;
using discipline.users.domain.Users.Rules.Users;

namespace discipline.users.domain.Users.ValueObjects.Users;

public sealed class FullName : ValueObject
{
    private readonly string _firstName = null!;
    private readonly string _lastName = null!;
    
    public string FirstName
    {
        get => _firstName;
        private init
        {
            CheckRule(new FirstNameCanNotBeEmptyRule(value));
            CheckRule(new FirstNameMustBeFrom2To100LengthRule(value));
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        private init
        {
            CheckRule(new LastNameCanNotBeEmptyRule(value));
            CheckRule(new LastNameMustBeFrom2To100LengthRule(value));
            _lastName = value;
        }
    }

    public static FullName Create(string firstName, string lastName)
        => new (firstName, lastName);
    
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
    }
}