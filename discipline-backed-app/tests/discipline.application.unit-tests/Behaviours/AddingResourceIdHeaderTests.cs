using discipline.application.Behaviours;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours;

public sealed class AddingResourceIdHeaderTests
{
    [Fact]
    public void AddResourceIdHeader_GivenId_ShouldAddToResponseHeader()
    {
        //arrange
        var httpContext = Substitute.For<HttpContext>();
        var httpResponse = Substitute.For<HttpResponse>();
        var headers = new HeaderDictionary();

        httpContext.Response.Returns(httpResponse);
        httpResponse.Headers.Returns(headers);

        var id = Guid.NewGuid();
        
        //act
        httpContext.AddResourceIdHeader(id);
        
        //assert
        headers.TryGetValue(AddingResourceIdHeader.HeaderName, out var value).ShouldBeTrue();
        value.Any(x => value == id.ToString()).ShouldBeTrue();
    }
}