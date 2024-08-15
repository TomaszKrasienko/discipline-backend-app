using System.Net;
using discipline.api.integration_tests._Helpers;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.ActivityRules;

[Collection("integration-tests")]
public sealed class DeleteActivityRuleTests : BaseTestsController
{
    [Fact]
    public async Task Delete_GivenExistingActivityRuleId_ShouldReturn200OkStatusCodeAndRemoveActivityRule()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var activityRule = ActivityRuleFactory.Get();
        await TestAppDb.GetCollection<ActivityRuleDocument>().InsertOneAsync(activityRule.AsDocument());
        
        //act
        var response = await HttpClient.DeleteAsync($"activity-rules/{activityRule.Id.Value}/delete");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var isActivityRuleExists = await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id.Equals(activityRule.Id))
            .AnyAsync();
        isActivityRuleExists.ShouldBeFalse();
    }

    [Fact]
    public async Task Delete_GivenNotExistingActivityRuleId_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        
        //act
        var response = await HttpClient.DeleteAsync($"activity-rules/{Guid.NewGuid()}/delete");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Delete_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.DeleteAsync($"activity-rules/{Guid.NewGuid()}/delete");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Delete_AuthorizedByUserWithStatusCreated_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        
        //act
        var response = await HttpClient.DeleteAsync($"activity-rules/{Guid.NewGuid()}/delete");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}