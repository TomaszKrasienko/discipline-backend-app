using System.Net;
using discipline.api.integration_tests._Helpers;
using discipline.tests.shared.Entities;
using Microsoft.EntityFrameworkCore;
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
        var activityRule = ActivityRuleFactory.Get();
        await DbContext.ActivityRules.AddAsync(activityRule);
        await DbContext.SaveChangesAsync();
        
        //act
        var response = await HttpClient.DeleteAsync($"activity-rules/{activityRule.Id.Value}/delete");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var isActivityRuleExist = await DbContext
            .ActivityRules
            .AsNoTracking()
            .AnyAsync(x => x.Id.Equals(activityRule.Id));
        isActivityRuleExist.ShouldBeFalse();
    }

    [Fact]
    public async Task Delete_GivenNotExistingActivityRuleId_ShouldReturn400BadRequestStatusCode()
    {
        //act
        var response = await HttpClient.DeleteAsync($"activity-rules/{Guid.NewGuid()}/delete");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}