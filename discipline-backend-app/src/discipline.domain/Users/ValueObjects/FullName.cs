using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed record FullName
{
    public string FirstName { get; }
    public string LastName { get; }

    public FullName(string firstName, string lastName)
    {
        //TODO: maybe policy?
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new EmptyUserFirstNameException();
        }
        
        if(firstName.Length is < 3 or > 100)
        {
            throw new InvalidUserFirstNameException(firstName);
        }
        
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new EmptyUserLastNameException();
        }

        if (lastName.Length is < 3 or > 100)
        {
            throw new InvalidUserLastNameException(lastName);
        }
        
        FirstName = firstName;
        LastName = lastName;
    }
}