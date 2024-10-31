using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.ValueObjects.Users;
using Shouldly;
using Xunit;

namespace discipline.domain.unit_tests.Users.ValueObjects.Users;

public sealed class FullNameTests
{
    [Theory]
    [MemberData(nameof(GetValidFullName))]
    public void Create_GivenValidFullName_ShouldReturnFullNameWithFirstNameAndLastName(string firstName,
        string lastName)
    {
        //act
        var result = FullName.Create(firstName, lastName);
        
        //assert
        result.FirstName.ShouldBe(firstName);
        result.LastName.ShouldBe(lastName);
    }

    public static IEnumerable<object[]> GetValidFullName()
    {
        yield return [new string('t', 2), new string('t', 2)];
        yield return [new string('t', 100), new string('t', 100)];
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidFullName))]
    public void Create_GivenInvalidFullName_ShouldThrowDomainExceptionWithCode(string firstName, string lastName,
        string code)
    {
        //act
        var exception = Record.Exception(() => FullName.Create(firstName, lastName));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    public static IEnumerable<object[]> GetInvalidFullName()
    {
        yield return [ string.Empty, "test_last_name", "User.FullName.FirstName.Empty" ];
        yield return [ "test_first_name", string.Empty, "User.FullName.LastName.Empty" ];
        yield return [ new string('t', 1), "test_last_name", "User.FullName.FirstName.InvalidLength" ];
        yield return [ new string('t', 101), "test_last_name", "User.FullName.FirstName.InvalidLength" ];
        yield return [ "test_first_name", new string('t', 1), "User.FullName.LastName.InvalidLength" ];
        yield return [ "test_first_name", new string('t', 101), "User.FullName.LastName.InvalidLength" ];
    }
}