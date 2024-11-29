using System.Net;
using System.Net.Http.Json;
using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.integration_tests.shared;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integration_tests;

[Collection("activity-rules-module-edit-activity-rule")]
public sealed class UpdateActivityRuleTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task Update_GivenExistingActivityRuleWithValidArguments_ShouldReturn204NoContentStatusCodeAndUpdateActivityRule()
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var activityRuleDocument = activityRule.MapAsDocument();
        activityRuleDocument.UserId = user.Id.ToString();

        await TestAppDb.GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRuleDocument);
        
        var command = new UpdateActivityRuleCommand(activityRule.Id, activityRule.UserId,"new_test_title",
            Mode.CustomMode, [1]);

        //act
        var response = await HttpClient.PutAsJsonAsync($"activity-rules-module/activity-rules/{activityRule.Id.ToString()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
        var updatedActivityRuleDocument = await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id.ToString() == activityRule.Id.ToString())
            .SingleOrDefaultAsync(); 
        updatedActivityRuleDocument.Title.ShouldBe(command.Title);
        updatedActivityRuleDocument.Mode.ShouldBe(command.Mode);
        updatedActivityRuleDocument.SelectedDays!.First().ShouldBe(command.SelectedDays![0]);
    }
    
    [Fact]
    public async Task Update_GivenExistingActivityRuleWithInvalidArguments_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();
        await TestAppDb.GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRule.MapAsDocument());

        await AuthorizeWithFreeSubscriptionPicked();
        var command = new UpdateActivityRuleDto( activityRule.Title, "test", null);

        //act
        var response = await HttpClient.PutAsJsonAsync($"activity-rules-module/activity-rules/{activityRule.Id.ToString()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Update_GivenInvalidCommand_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new UpdateActivityRuleDto(string.Empty, Mode.EveryDayMode, null);
        await AuthorizeWithFreeSubscriptionPicked();
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
    
    [Fact]
    public async Task Update_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new UpdateActivityRuleDto("test_title", Mode.EveryDayMode, null);

        //act
        var response = await HttpClient.PutAsJsonAsync($"activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Update_AuthorizedByUserWithoutSubscription_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        var command = new UpdateActivityRuleDto("test_title", Mode.EveryDayMode, null);
        await AuthorizeWithoutSubscription();
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}