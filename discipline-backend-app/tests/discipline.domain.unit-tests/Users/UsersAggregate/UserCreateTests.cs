using System.Collections;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Events;
using discipline.domain.Users.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users;

public sealed class UserCreateTests
{
    [Fact]
    public void Create_GivenAllValidArguments_ShouldReturnUserWithAllFillFieldsAndDomainEvent()
    {
        //arrange
        var id = UserId.New();
        var email = "test@test.pl";
        var password = "test_password";
        var firstName = "test_first_name";
        var lastName = "test_last_name";
        
        //act
        var result = User.Create(id, email, password, firstName, lastName);
        
        //assert
        result.Id.ShouldBe(id);
        result.Email.Value.ShouldBe(email);
        result.Password.Value.ShouldBe(password);
        result.FullName.FirstName.ShouldBe(firstName);
        result.FullName.LastName.ShouldBe(lastName);
        result.Status.Value.ShouldBe("Created");
        result.DomainEvents.Any(x => x is UserCreated).ShouldBeTrue();
    }

    [Theory]
    [MemberData(nameof(GetUserCreateInvalidData))]
    public void Create_GivenInvalidArgument_ShouldThrowDomainException(UserCreateParameters parameters)
    {
        //act
        var exception = Record.Exception(() => User.Create(parameters.UserId, parameters.Email,
            parameters.Password, parameters.FirstName, parameters.LastName));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }

    public static IEnumerable<object[]> GetUserCreateInvalidData()
    {
        yield return [new UserCreateParameters(UserId.New(), string.Empty,
                "test_password", "test_first_name", "test_last_name")];
        
        yield return [new UserCreateParameters(UserId.New(), "test_email",
            "test_password", "test_first_name", "test_last_name")];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", string.Empty, "test_last_name")];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", new string('t', 1), "test_last_name")];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", new string('t', 101), "test_last_name")];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", "test_first_name",string.Empty)];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", "test_first_name", new string('t', 1))];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", "test_first_name", new string('t', 101))];
    }

    public sealed record UserCreateParameters(UserId UserId, string Email, string Password,
        string FirstName, string LastName);
}