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
public sealed class CreateActivityRuleTests : BaseTestsController
{
    [Fact]
    public async Task Create_GivenValidArguments_ShouldReturn201CreatedStatusCode()
    {
        //arrange
        var command = new CreateActivityRuleCommand(Guid.Empty, "Test title", Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PostAsJsonAsync<CreateActivityRuleCommand>("/activity-rule/create", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var isActivityExists = await DbContext.ActivityRules.AnyAsync(x => x.Id.Equals(resourceId));
        isActivityExists.ShouldBeTrue();
    }
    
    [Fact]
    public async Task Create_GivenAlreadyExistingTitle_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var activityRule = ActivityRuleFactory.Get();
        await DbContext.ActivityRules.AddAsync(activityRule);
        await DbContext.SaveChangesAsync();
        var command = new CreateActivityRuleCommand(Guid.Empty, activityRule.Title, Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PostAsJsonAsync<CreateActivityRuleCommand>("/activity-rule/create", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Create_GivenInvalidRequest_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new CreateActivityRuleCommand(Guid.Empty, string.Empty, Mode.EveryDayMode(), null);
        
        //act
        var response = await HttpClient.PostAsJsonAsync<CreateActivityRuleCommand>("/activity-rule/create", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}