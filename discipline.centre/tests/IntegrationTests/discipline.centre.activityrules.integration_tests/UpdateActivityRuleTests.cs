using System.Net;
using System.Net.Http.Json;
using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
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
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var activityRuleDocument = activityRule.MapAsDocument();

        await TestAppDb.GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRuleDocument with {UserId = user.Id.ToString()});

        var request = new UpdateActivityRuleDto(new ActivityRuleDetailsSpecification("new_test_title", "new_test_note"),
            Mode.CustomMode, [0]);

        //act
        var response = await HttpClient.PutAsJsonAsync($"activity-rules-module/activity-rules/{activityRule.Id.ToString()}", request);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
        var updatedActivityRuleDocument = await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id.ToString() == activityRule.Id.ToString())
            .SingleOrDefaultAsync(); 
        updatedActivityRuleDocument.Title.ShouldBe(request.Details.Title);
        updatedActivityRuleDocument.Note.ShouldBe(request.Details.Note);
        updatedActivityRuleDocument.Mode.ShouldBe(request.Mode);
        updatedActivityRuleDocument.SelectedDays!.First().ShouldBe(request.SelectedDays![0]);
    }
    
    [Fact]
    public async Task Update_GivenExistingActivityRuleWithInvalidArguments_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        await TestAppDb.GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRule.MapAsDocument());

        await AuthorizeWithFreeSubscriptionPicked();
        var request = new UpdateActivityRuleDto( new ActivityRuleDetailsSpecification("new_test_title",
            null), "test", null);

        //act
        var response = await HttpClient.PutAsJsonAsync($"activity-rules-module/activity-rules/{activityRule.Id.ToString()}", request);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Update_GivenInvalidCommand_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var request = new UpdateActivityRuleDto(new ActivityRuleDetailsSpecification(string.Empty, null),
            Mode.EveryDayMode, null);
        await AuthorizeWithFreeSubscriptionPicked();
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}", request);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
    
    [Fact]
    public async Task Update_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var request = new UpdateActivityRuleDto(new ActivityRuleDetailsSpecification("new_test_title", null),
            Mode.EveryDayMode, null);

        //act
        var response = await HttpClient.PutAsJsonAsync($"activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}", request);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Update_AuthorizedByUserWithoutSubscription_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        var request = new UpdateActivityRuleDto(new ActivityRuleDetailsSpecification("test_title", null),
            Mode.EveryDayMode, null);
        await AuthorizeWithoutSubscription();
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}", request);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}