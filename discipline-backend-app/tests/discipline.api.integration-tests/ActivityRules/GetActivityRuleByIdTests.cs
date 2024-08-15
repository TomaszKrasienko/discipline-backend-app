using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.ActivityRules;

[Collection("integration-tests")]
public sealed class GetActivityRuleByIdTests : BaseTestsController
{
    [Fact]
    public async Task GetActivityRuleById_GivenExistingActivityRule_ShouldReturnActivityDto()
    {
        //arrange 
        await AuthorizeWithFreeSubscriptionPicked();
        var activityRule = ActivityRuleFactory.Get();
        await TestAppDb.GetCollection<ActivityRuleDocument>().InsertOneAsync(activityRule.AsDocument());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<ActivityRuleDto>($"/activity-rules/{activityRule.Id.Value}");
        
        //assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(activityRule.Id.Value);
    }
    
    [Fact]
    public async Task GetActivityRuleById_GivenNotExistingActivityRule_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        
        //act
        var response = await HttpClient.GetAsync($"/activity-rules/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetActivityRuleById_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"/activity-rules/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetActivityRuleById_AuthorizedByUserWithStatusCreated_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        
        //act
        var response = await HttpClient.GetAsync($"/activity-rules/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}