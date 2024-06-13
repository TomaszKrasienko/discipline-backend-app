using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.ActivityRuleModes;

public class GetActivityRuleModesTests : BaseTestsController
{
    [Fact]
    public async Task GetActivityRuleModes_Always_ShouldReturnActivityRuleModeDtoList()
    {
        //act
        var result = await HttpClient.GetFromJsonAsync<List<ActivityRuleModeDto>>($"/activity-rule-modes");
        
        //assert
        result.ShouldNotBeNull();
    }
}