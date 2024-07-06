using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.application.Features.ActivityRules;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
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
        var activityRule = ActivityRuleFactory.Get();
        await TestAppDb.GetCollection<ActivityRuleDocument>("ActivityRules").InsertOneAsync(activityRule.AsDocument());
        var command = new EditActivityRuleCommand(Guid.Empty, "NewTitle", Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rules/{activityRule.Id.Value}/edit",
                command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = (await TestAppDb
            .GetCollection<ActivityRuleDocument>("ActivityRules")
            .Find(x => x.Id.Equals(activityRule.Id))
            .FirstOrDefaultAsync()).AsEntity();

        result.ShouldNotBeNull();
        result.Title.Value.ShouldBe(command.Title);
        result.Mode.Value.ShouldBe(command.Mode);
    }
    
    [Fact]
    public async Task Edit_GivenValidArgumentsAndNotExistingActivityRule_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new EditActivityRuleCommand(Guid.Empty, "NewTitle", Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rules/{Guid.NewGuid()}/edit",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Edit_GivenInvalidArguments_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new EditActivityRuleCommand(Guid.Empty, string.Empty, Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rules/{Guid.NewGuid()}/edit",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}