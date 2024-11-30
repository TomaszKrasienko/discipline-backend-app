using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Users.ValueObjects.Users;
using Shouldly;
using Xunit;

namespace discipline.centre.domain.unit_tests.Users.ValueObjects.Users;

public sealed class PasswordTests
{
    [Fact]
    public void Create_GivenValidValue_ShouldReturnPasswordWithValue()
    {
        //arrange
        var value = "Test123!";
        
        //act
        var result = Password.Create(value);
        
        //assert
        result.Value.ShouldBe(value);
    }

    [Theory]
    [MemberData(nameof(GetInvalidPasswordData))]
    public void Create_GivenInvalidValue_ShouldThrowDomainExceptionWithCode(string password, string code)
    {
        //act
        var exception = Record.Exception(() => Password.Create(password));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    public static IEnumerable<object[]> GetInvalidPasswordData()
    {
        yield return ["123", "User.Password.InvalidLength"];
        yield return ["Test1234", "User.Password.SpecialCharacters"];
        yield return ["TestTest", "User.Password.SpecialCharacters"];
        yield return ["test123!", "User.Password.SpecialCharacters"];
        yield return ["TEST123!", "User.Password.SpecialCharacters"];
    }
}