using discipline.centre.dailytrackers.application.DailyTrackers.Services;
using discipline.centre.dailytrackers.infrastructure.Storage;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.infrastructure.unit_tests.Storage;

public sealed class HttpContextActivityIdStorageTests
{
    [Fact]
    public void Set_GivenActivityId_ShouldSetActivityIdInHttpContext()
    {
        //arrange
        var activityId = ActivityId.New();
        
        //act
        _storage.Set(activityId);
        
        //assert
        _httpContextAccessor.HttpContext!.Items.TryGetValue("activity_id", out var result).ShouldBeTrue();
        result.ShouldBe(activityId);
    }

    [Fact]
    public void Get_GivenActivityIdInHttpContext_ShouldReturnActivityId()
    {
        //arrange
        var activityId = ActivityId.New();
        _httpContextAccessor.HttpContext!.Items.Add("activity_id", activityId);
        
        //act
        var result = _storage.Get();
        
        //assert
        result.ShouldNotBeNull();
        result.ShouldBe(activityId);
    }

    [Fact]
    public void Get_NotGivenActivityIdInHttpContext_ShouldReturnNull()
    {
        //act
        var result = _storage.Get();
        
        //assert
        result.ShouldBeNull();
    }

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IActivityIdStorage _storage;

    public HttpContextActivityIdStorageTests()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _httpContextAccessor.HttpContext = new DefaultHttpContext();
        _storage = new HttpContextActivityIdStorage(_httpContextAccessor);
    }
}