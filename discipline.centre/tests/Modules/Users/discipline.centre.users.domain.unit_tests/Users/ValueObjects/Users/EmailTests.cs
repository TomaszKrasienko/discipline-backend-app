using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Users.ValueObjects.Users;
using Shouldly;
using Xunit;

namespace discipline.centre.domain.unit_tests.Users.ValueObjects.Users;

public sealed class EmailTests
{
    [Fact]
    public void Create_GivenValidEmail_ShouldReturnEmailWithValue()
    {
        //arrange
        var value = "test@test.pl";
        
        //act
        var result = Email.Create(value);
        
        //assert
        result.Value.ShouldBe(value);
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidEmailsWithCodes))]
    public void Create_GivenInvalidEmail_ShouldThrowDomainEventWithCode(string email, string code)
    {
        //act
        var exception = Record.Exception(() => Email.Create(email));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    public static IEnumerable<object[]> GetInvalidEmailsWithCodes()
    {
        yield return
        [
            string.Empty, "User.Email.Empty"
        ];
        yield return
        [
            "test", "User.Email.Invalid"
        ];
    }
}