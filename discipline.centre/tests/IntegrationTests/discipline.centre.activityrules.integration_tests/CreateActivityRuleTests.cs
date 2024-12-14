using System.Net;
using System.Net.Http.Json;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.integration_tests.shared;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integration_tests;

[Collection("activity-rules-module-create-activity-rule")]
public sealed class CreateActivityRuleTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task Create_GivenValidParameters_ShouldReturn201CreatedStatusCodeAndAddToDb()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var command = new CreateActivityRuleDto( new ActivityRuleDetailsSpecification("test_title", 
            "test_note"), Mode.EveryDayMode, null, null);
         
        //act
        var response = await HttpClient.PostAsJsonAsync("/activity-rules-module/activity-rules", command);
         
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
         
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();

        var newActivityRuleDocument = await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id.ToString() == resourceId)
            .SingleOrDefaultAsync(); 

        newActivityRuleDocument.ShouldNotBeNull();
        newActivityRuleDocument.UserId.ShouldBe(user.Id.ToString());
    }
    
    [Fact]
    public async Task Create_GivenAlreadyExistingTitle_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var activityRuleDocument = activityRule.MapAsDocument();
            
        await TestAppDb.GetCollection<ActivityRuleDocument>().InsertOneAsync(activityRuleDocument with { UserId = user.Id.ToString() });
        var command = new CreateActivityRuleDto(new ActivityRuleDetailsSpecification(activityRule.Details.Title,
            null), Mode.EveryDayMode, null, null);
         
        //act
        var response = await HttpClient.PostAsJsonAsync("/activity-rules-module/activity-rules", command);
         
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Create_GivenInvalidRequest_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new CreateActivityRuleDto(new ActivityRuleDetailsSpecification(string.Empty, 
            null), Mode.EveryDayMode, null, null);
         
        //act
        var response = await HttpClient.PostAsJsonAsync("/activity-rules-module/activity-rules", command);
         
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task Create_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new CreateActivityRuleDto(new ActivityRuleDetailsSpecification("test_title", 
            null), Mode.EveryDayMode, null, null);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/activity-rules-module/activity-rules", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
     
    [Fact]
    public async Task Create_AuthorizedByUserWithStatusCreated_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new CreateActivityRuleDto(new ActivityRuleDetailsSpecification("test_title", null),
            Mode.EveryDayMode, null, null);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/activity-rules-module/activity-rules", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}