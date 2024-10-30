using System.Text;
using discipline.application.Behaviours.Passwords;
using discipline.domain.Users;
using discipline.infrastructure.Passwords;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace discipline.infrastructure.unit_tests.Password;

public sealed class PasswordSecureBehaviourTests
{
    [Fact]
    public void Secure_GivenPassword_ShouldReturnSecuredPassword()
    {
        //arrange
        var password = "test_password";
        
        //act
        var result = _passwordManager.Secure(password);
        
        //assert
        result.ShouldNotBe(password);
    }

    [Fact]
    public void VerifyPassword_GivenValidPassword_ShouldReturnTrue()
    {
        //arrange
        var password = "test_password";
        var securedPassword = _passwordManager.Secure(password);
        
        //act
        var result = _passwordManager.VerifyPassword(securedPassword, password);
        
        //assert
        result.ShouldBeTrue();
    }
    
    
    [Fact]
    public void VerifyPassword_GivenInvalidPassword_ShouldReturnFalse()
    {
        //arrange
        var password = "test_password";
        var securedPassword = _passwordManager.Secure(password);
        
        //act
        var result = _passwordManager.VerifyPassword(Convert.ToBase64String(
            Encoding.UTF8.GetBytes("invalid_password")), password);
        
        //assert
        result.ShouldBeFalse();
    }
    
    #region arrange
    private readonly IPasswordManager _passwordManager;

    public PasswordSecureBehaviourTests()
    {
        var passwordHasher = new PasswordHasher<User>();
        _passwordManager = new PasswordManager(passwordHasher);
    }

    #endregion
}