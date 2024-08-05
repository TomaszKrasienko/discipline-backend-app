using System.Security.Claims;
using discipline.application.Behaviours;
using discipline.application.Domain.Users.Entities;
using Microsoft.AspNetCore.Http;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours.IdentityFromContextBehaviour;

public class IdentityContextTests
{
    [Fact]
    public void New_GivenAuthenticatedContextWithNameAsGuidAndStatus_ShouldReturnIdentityContext()
    {
        //arrange
        var name = Guid.NewGuid();
        var status = "test_status";
        
        var claims = new List<Claim>() 
        { 
            new Claim(ClaimTypes.Name, name.ToString()),
            new Claim("Status", status)
        };
        var identity = new ClaimsIdentity(claims, "authentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext()
        {
            User = claimsPrincipal
        };
        
        //act
        var result = new IdentityContext(httpContext);
        
        //assert
        result.IsAuthenticated.ShouldBeTrue();
        result.UserId.ShouldBe(name);
        result.Status.ShouldBe(status);
    }
    
    [Fact]
    public void New_GivenNotAuthenticatedContext_ShouldReturnIdentityContextWithFalseIsAuthenticated()
    {
        //arrange
        var claimsPrincipal = new ClaimsPrincipal();

        var httpContext = new DefaultHttpContext()
        {
            User = claimsPrincipal
        };
        
        //act
        var result = new IdentityContext(httpContext);
        
        //assert
        result.IsAuthenticated.ShouldBeFalse();
    }

    [Fact]
    public void New_GivenIdentityNameNotAsGuid_ShouldThrowArgumentException()
    {
        //arrange
        var claims = new List<Claim>() 
        { 
            new Claim(ClaimTypes.Name, "username"),
        };
        var identity = new ClaimsIdentity(claims, "authentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext()
        {
            User = claimsPrincipal
        };
        
        //act
        var exception = Record.Exception(() => new IdentityContext(httpContext));
        
        //assert
        exception.ShouldBeOfType<ArgumentException>();
    }
}