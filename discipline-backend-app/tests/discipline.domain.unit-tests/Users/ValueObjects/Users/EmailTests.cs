using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.ValueObjects.Users;
using Shouldly;
using Xunit;

namespace discipline.domain.unit_tests.Users.ValueObjects.Users;

public sealed class EmailTests
{
    [Theory]
    [MemberData(nameof(GetInvalidEmailsWithCodes))]
    public void New_GivenEmail_ShouldThrowDomainEventWithCode(string email, string code)
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