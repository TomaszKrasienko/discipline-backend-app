using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.application.Features.ActivityRules;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.ActivityRules;

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
    }
}