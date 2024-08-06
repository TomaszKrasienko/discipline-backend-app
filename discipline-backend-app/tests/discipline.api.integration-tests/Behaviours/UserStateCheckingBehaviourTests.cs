using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.application.Domain.Users.ValueObjects;
using discipline.application.Features.ActivityRules;
using Microsoft.AspNetCore.Http;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.Behaviours;

public sealed class UserStateCheckingBehaviourTests : BaseTestsController 
{
    [Fact]
    public async Task CreateActivityRule_GivenNotExistingUserId_ShouldReturnStatusCode401()
    {
        //arrange
        Authorize(Guid.NewGuid(), Status.PaidSubscriptionPicked());
        var command = new CreateActivityRuleCommand(Guid.Empty, Guid.Empty, "test_title",
            Mode.EveryDayMode(), null);
        
        //act
        var result = await HttpClient.PostAsJsonAsync("activity-rules/create", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}