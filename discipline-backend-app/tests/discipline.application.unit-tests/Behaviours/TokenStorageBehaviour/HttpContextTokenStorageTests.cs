using discipline.application.Behaviours;
using discipline.application.DTOs;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours.TokenStorageBehaviour;

public sealed class HttpContextTokenStorageTests
{
    [Fact]
    public void Set_GivenJwtDto_ShouldSaveTokenInHttpContext()
    {
        //arrange
        var jwtDto = new JwtDto()
        {
            Token = Guid.NewGuid().ToString()
        };
        
        //act
        _tokenStorage.Set(jwtDto);
        
        //assert
        object savedJwtDto = new JwtDto();
        var isExists = _httpContextAccessor?
            .HttpContext?
            .Items.TryGetValue("user_jwt_token", out savedJwtDto!);
        isExists!.Value.ShouldBeTrue();
        ((JwtDto)savedJwtDto).Token.ShouldBe(jwtDto.Token);
    }

    [Fact]
    public void Get_GivenExistingJwtDto_ShouldReturnJwtDto()
    {
        //arrange
        var jwtDto = new JwtDto()
        {
            Token = Guid.NewGuid().ToString()
        };
        _httpContextAccessor?.HttpContext?.Items.TryAdd("user_jwt_token", jwtDto);
        
        //act
        var result = _tokenStorage.Get();
        
        //assert
        result.Token.ShouldBe(jwtDto.Token);
    }

    [Fact]
    public void Get_GivenNotExistingJwtDto_ShouldReturnNull()
    {
        //act
        var result = _tokenStorage.Get();
        
        //assert
        result.ShouldBeNull();
    }
    
    #region arrange
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenStorage _tokenStorage;

    public HttpContextTokenStorageTests()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _httpContextAccessor.HttpContext = new DefaultHttpContext();
        _tokenStorage = new HttpContextTokenStorage(_httpContextAccessor);
    }
    #endregion
}