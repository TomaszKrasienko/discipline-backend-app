using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours.ResourceHeader;

public sealed class AddingResourceIdHeaderBehaviourTests
{
    [Fact]
    public void AddResourceIdHeader_GivenId_ShouldAddToResponseHeader()
    {
        //arrange
        var accessor = Substitute.For<IHttpContextAccessor>();
        var httpContext = Substitute.For<HttpContext>();
        var httpResponse = Substitute.For<HttpResponse>();
        var headers = new HeaderDictionary();

        accessor.HttpContext.Returns(httpContext);
        httpContext.Response.Returns(httpResponse);
        httpResponse.Headers.Returns(headers);

        var id = Guid.NewGuid();
        
        //act
        accessor.AddResourceIdHeader(id.ToString());
        
        //assert
        headers.TryGetValue(ResourceHeaderExtension.HeaderName, out var value).ShouldBeTrue();
        value.Any(x => value == id.ToString()).ShouldBeTrue();
    }
}