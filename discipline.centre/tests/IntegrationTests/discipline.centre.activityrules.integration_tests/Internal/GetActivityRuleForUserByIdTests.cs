using System.Net;
using System.Net.Http.Json;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.integration_tests.shared;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integration_tests.Internal;

[Collection("activity-rules-module-get-activity-rule-for-user-id")]
public sealed class GetActivityRuleForUserByIdTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task GetActivityRuleForUserById_GivenExistingActivityRuleForUser_ShouldReturn200OkStatusCodeWithActivityRuleDto()
    {
        //arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        await TestAppDb.GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRule.MapAsDocument());
        
        Authorize();
        
        //act
        var response = await HttpClient.GetAsync($"activity-rules-module/activity-rules-internal/{activityRule.UserId.Value}/{activityRule.Id}");
            
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        
        
        var result = await response.Content.ReadFromJsonAsync<ActivityRuleDto>();
    }
}