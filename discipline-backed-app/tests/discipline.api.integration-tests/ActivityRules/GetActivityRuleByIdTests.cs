using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
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
        var activityRule = ActivityRuleFactory.Get();
        await DbContext.ActivityRules.AddAsync(activityRule);
        await DbContext.SaveChangesAsync();
        
        //act
        var result = await HttpClient.GetFromJsonAsync<ActivityRuleDto>($"/activity-rule/{activityRule.Id.Value}");
        
        //assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(activityRule.Id.Value);
    }
    
    [Fact]
    public async Task GetActivityRuleById_GivenNotExistingActivityRule_ShouldReturn204NoContentStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"/activity-rule/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}