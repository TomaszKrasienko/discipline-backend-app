using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Users.ValueObjects.Users;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unit_tests.Users.ValueObjects.Users;

public sealed class StatusTests
{
    [Theory]
    [MemberData(nameof(GetValidStatus))]
    public void Create_GivenValidStatus_ShouldReturnStatus(string status)
    {
        //act
        var result = Status.Create(status);
        
        //assert
        result.Value.ShouldBe(status);
    }

    public static IEnumerable<object[]> GetValidStatus()
    {
        yield return [Status.Created];
        yield return [Status.PaidSubscriptionPicked];
        yield return [Status.FreeSubscriptionPicked];
    }

    [Theory]
    [MemberData(nameof(GetInvalidStatus))]
    public void Create_GivenInvalidStatus_ShouldThrowDomainExceptionWithCode(string status, string code)
    {
        //act
        var exception = Record.Exception(() => Status.Create(status));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    public static IEnumerable<object[]> GetInvalidStatus()
    {
        yield return [string.Empty, "User.Status.Empty"];
        yield return ["test", "User.Status.Unavailable"];
    }
}