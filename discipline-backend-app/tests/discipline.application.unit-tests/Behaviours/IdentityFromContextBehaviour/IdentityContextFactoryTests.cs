using System.Security.Claims;
using discipline.application.Behaviours;
using Microsoft.AspNetCore.Http;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours.IdentityFromContextBehaviour;

public sealed class IdentityContextFactoryTests
{
    [Fact]
    public void Create_GivenHttpContextAccessorByConstructor_ShouldReturnIIdentityContext()
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
        var contextAccessor = new HttpContextAccessor()
        {
            HttpContext = httpContext
        };
        var identityContextFactory = new IdentityContextFactory(contextAccessor);
        
        //act
        var result = identityContextFactory.Create();
        
        //assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<IIdentityContext>();
        result.IsAuthenticated.ShouldBeTrue();
    }
}