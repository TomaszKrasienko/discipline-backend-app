using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities.Create;

public sealed class UserCreateTests
{
    [Fact]
    public void Create_GivenAllValidArguments_ShouldReturnUserWithAllFillFields()
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
    }
    
    [Fact]
    public void Create_GivenEmptyEmail_ShouldThrowEmptyUserEmailException()
    {
        //act
        var exception = Record.Exception(() => User.Create(UserId.New(), string.Empty,
            "test_password", "test_first_name", "test_last_name"));
        
        //assert
        exception.ShouldBeOfType<EmptyUserEmailException>();
    }

    [Fact]
    public void Create_GivenInvalidEmail_ShouldThrowInvalidUserEmailException()
    {
        //act
        var exception = Record.Exception(() => User.Create(UserId.New(), "test_email",
            "test_password", "test_first_name", "test_last_name"));
        
        //assert
        exception.ShouldBeOfType<InvalidUserEmailException>();
    }

    [Fact]
    public void Create_GivenEmptyUserFirstName_ShouldThrowEmptyUserFirstNameException()
    {
        //act
        var exception = Record.Exception(() => User.Create(UserId.New(), "test@test.pl",
            "test_password", string.Empty, "test_last_name"));
        
        //assert
        exception.ShouldBeOfType<EmptyUserFirstNameException>();
    }

    [Theory]
    [InlineData('t', 2)]
    [InlineData('t', 101)]
    public void Create_GivenInvalidFirstName_ShouldThrowInvalidUserFirstNameException(char character, int multiplier)
    {
        //arrange
        var firstName = new string(character, multiplier);
        
        //act
        var exception = Record.Exception(() => User.Create(UserId.New(), "test@test.pl",
            "test_password", firstName, "test_last_name"));
        
        //assert
        exception.ShouldBeOfType<InvalidUserFirstNameException>();
    }
    
    [Fact]
    public void Create_GivenEmptyLastFirstName_ShouldThrowEmptyUserLastNameException()
    {
        //act
        var exception = Record.Exception(() => User.Create(UserId.New(), "test@test.pl",
            "test_password", "test_first_name",string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyUserLastNameException>();
    }
    
    [Theory]
    [InlineData('t', 2)]
    [InlineData('t', 101)]
    public void Create_GivenInvalidLastName_ShouldThrowInvalidUserLastNameException(char character, int multiplier)
    {
        //arrange
        var lastName = new string(character, multiplier);
        
        //act
        var exception = Record.Exception(() => User.Create(UserId.New(), "test@test.pl",
            "test_password", "test_first_name", lastName));
        
        //assert
        exception.ShouldBeOfType<InvalidUserLastNameException>();
    }
}