using System.Security.Claims;
using discipline.domain.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.infrastructure.unit_tests.IdentityContext;

public class IdentityContextTests
{
    [Fact]
    public void New_GivenAuthenticatedContextWithNameAsGuidAndStatus_ShouldReturnIdentityContext()
    {
        //arrange
        var name = UserId.New();
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

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);
        
        //act
        var result = new infrastructure.IdentityContext.IdentityContext(httpContextAccessor);
        
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
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);
        
        //act
        var result = new infrastructure.IdentityContext.IdentityContext(httpContextAccessor);
        
        //assert
        result.IsAuthenticated.ShouldBeFalse();
    }

    [Fact]
    public void New_GivenIdentityNameNotAsUlid_ShouldThrowArgumentException()
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
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);
        
        //act
        var exception = Record.Exception(() => new infrastructure.IdentityContext.IdentityContext(httpContextAccessor));
        
        //assert
        exception.ShouldBeOfType<ArgumentException>();
    }
}