using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.ActivityRules;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.ActivityRules;

[Collection("integration-tests")]
public class EditActivityRuleTests : BaseTestsController
{
    [Fact]
    public async Task Edit_GivenValidArgumentsAndExistingActivityRule_ShouldReturn200OkStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var activityRule = ActivityRuleFactory.Get();
        await TestAppDb.GetCollection<ActivityRuleDocument>().InsertOneAsync(activityRule.AsDocument());
        var command = new EditActivityRuleCommand(new ActivityRuleId(Ulid.Empty), "NewTitle", Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rules/{activityRule.Id.Value}/edit",
                command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = (await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id == activityRule.Id.ToString())
            .FirstOrDefaultAsync()).AsEntity();

        result.ShouldNotBeNull();
        result.Title.Value.ShouldBe(command.Title);
        result.Mode.Value.ShouldBe(command.Mode);
    }
    
    [Fact]
    public async Task Edit_GivenValidArgumentsAndNotExistingActivityRule_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new EditActivityRuleCommand(new ActivityRuleId(Ulid.Empty), "NewTitle", Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rules/{Guid.NewGuid()}/edit",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Edit_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new EditActivityRuleCommand(new ActivityRuleId(Ulid.Empty), string.Empty, Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rules/{Guid.NewGuid()}/edit",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Edit_AuthorizedByUserWithStatusCreated_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new EditActivityRuleCommand(new ActivityRuleId(Ulid.Empty), string.Empty, Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rules/{Guid.NewGuid()}/edit",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Edit_GivenInvalidArguments_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new EditActivityRuleCommand(new ActivityRuleId(Ulid.Empty), string.Empty, Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rules/{Ulid.NewUlid()}/edit",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}