using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.DailyProductivities;
using discipline.domain.ActivityRules.Entities;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.infrastructure.DAL.Documents.ActivityRules;
using discipline.infrastructure.DAL.Documents.DailyProductivities;
using discipline.infrastructure.DAL.Documents.Mappers;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.DailyProductivities;

[Collection("integration-tests")]
public sealed class CreateActivityFromRuleTests : BaseTestsController
{
    [Fact]
    public async Task CreateActivityFromRule_GivenExistingActivityRule_ShouldReturn200OkStatusCodeAndAddActivity()
    {
        //arrange
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", Mode.EveryDayMode());
        await TestAppDb.GetCollection<ActivityRuleDocument>().InsertOneAsync(activityRule.AsDocument());
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new CreateActivityFromRuleCommand(new ActivityId(Ulid.Empty), activityRule.Id);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productivity/today/add-activity-from-rule", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var isExists = await TestAppDb
            .GetCollection<DailyProductivityDocument>()
            .Find(x 
                => x.Activities.Any(a 
                    => a.ParentRuleId == command.ActivityRuleId.ToString()
                    && a.Title == activityRule.Title.Value))
            .AnyAsync();
        isExists.ShouldBeTrue();
    }
    
    [Fact]
    public async Task CreateActivityFromRule_GivenNotExistingActivityRule_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new CreateActivityFromRuleCommand(new ActivityId(Ulid.Empty), ActivityRuleId.New());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productivity/today/add-activity-from-rule", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateActivityFromRule_Unauthorized_ShouldReturn401UnauthorizedSStatusCode()
    {
        //arrange
        var command = new CreateActivityFromRuleCommand(new ActivityId(Ulid.Empty), ActivityRuleId.New());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productivity/today/add-activity-from-rule", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task CreateActivityFromRule_AuthorizedByUserWithStatusCreated_ShouldReturnResponse403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new CreateActivityFromRuleCommand(new ActivityId(Ulid.Empty), ActivityRuleId.New());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productivity/today/add-activity-from-rule", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task CreateActivityFromRule_GivenInvalidCommand_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new CreateActivityFromRuleCommand(new ActivityId(Ulid.Empty), new ActivityRuleId(Ulid.Empty));
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productivity/today/add-activity-from-rule", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}