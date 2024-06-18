using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.application.Features.ActivityRules;
using discipline.tests.shared.Entities;
using Microsoft.EntityFrameworkCore;
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
        await DbContext.ActivityRules.AddAsync(activityRule);
        await DbContext.SaveChangesAsync();
        var command = new EditActivityRuleCommand(Guid.Empty, "NewTitle", Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rule/{activityRule.Id.Value}/edit",
                command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await DbContext
            .ActivityRules
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(activityRule.Id));

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
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rule/{Guid.NewGuid()}/edit",
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
        var response = await HttpClient.PutAsJsonAsync<EditActivityRuleCommand>($"/activity-rule/{Guid.NewGuid()}/edit",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}