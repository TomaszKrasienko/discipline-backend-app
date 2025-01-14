using System.Net;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.integration_tests.shared;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integration_tests.Public;

[Collection("activity-rules-module-delete-activity-rule")]
public sealed class DeleteActivityRuleTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task Delete_GivenExistingActivityRule_ShouldReturn204NoContentStatusCodeAndDeleteFromDb()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), user.Id, 
            new ActivityRuleDetailsSpecification("test_title",null), Mode.EveryDayMode);
        
        await TestAppDb.GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRule.MapAsDocument());
        
        //act
        var response = await HttpClient.DeleteAsync($"api/activity-rules-module/activity-rules/{activityRule.Id.ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var doesActivityRuleExist = await TestAppDb.GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id == activityRule.Id.ToString() && x.UserId == user.Id.ToString())
            .AnyAsync();
        doesActivityRuleExist.ShouldBeFalse();
    }
    
    [Fact]
    public async Task Delete_GivenNotExistingActivityRule_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        _ = await AuthorizeWithFreeSubscriptionPicked();
        var activityRuleId = ActivityRuleId.New().ToString();
        
        //act
        var response = await HttpClient.DeleteAsync($"api/activity-rules-module/activity-rules/{activityRuleId}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Delete_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var activityRuleId = ActivityRuleId.New().ToString();
        
        //act
        var response = await HttpClient.DeleteAsync($"api/activity-rules-module/activity-rules/{activityRuleId}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Delete_AuthorizedWithoutSubscription_ShouldReturn403UnauthorizedStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var activityRuleId = ActivityRuleId.New().ToString();
        
        //act
        var response = await HttpClient.DeleteAsync($"api/activity-rules-module/activity-rules/{activityRuleId}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}