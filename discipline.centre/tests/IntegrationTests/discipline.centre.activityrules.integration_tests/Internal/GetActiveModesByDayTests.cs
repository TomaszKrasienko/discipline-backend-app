using System.Net;
using System.Net.Http.Json;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.integration_tests.shared;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integration_tests.Internal;

[Collection("activity-rules-module-get-active-modules")]
public sealed class GetActiveModesByDayTests() : BaseTestsController("activity-rules-modules")
{
    [Fact]
    public async Task GetActiveModesByDay_Authorized_ShouldReturn200OkStatusCodeWithActiveModesDto()
    {
        //arrange
        Authorize();
        var date = DateOnly.FromDateTime(DateTime.Now);
        
        //act
        var response = await HttpClient.GetAsync($"activity-rules-module/activity-rules-internal/modes?day={date.ToString("yyyy-MM-dd")}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ActiveModesDto>();
        result.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetActiveModesByDay_Unauthorized_ShouldReturn401Unauthorized()
    {
        //arrange
        var date = DateOnly.FromDateTime(DateTime.Now);
        
        //act
        var response = await HttpClient.GetAsync($"activity-rules-module/activity-rules-internal/modes?day={date.ToString("yyyy-MM-dd")}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}