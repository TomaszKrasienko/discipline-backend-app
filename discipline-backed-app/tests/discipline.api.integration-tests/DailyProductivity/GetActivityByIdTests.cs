using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.DailyProductivity;

public sealed class GetActivityByIdTests : BaseTestsController
{
    [Fact]
    public async Task GetActivityById_GivenExistingActivity_ShouldReturnActivityDto()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        await DbContext.DailyProductivity.AddAsync(dailyProductivity);
        await DbContext.SaveChangesAsync();
        
        //act
        var result = await HttpClient.GetFromJsonAsync<ActivityDto>($"daily-productive/activities/{activity.Id.Value}");
        
        //assert
        result.Id.ShouldBe(activity.Id.Value);
        result.Title.ShouldBe(activity.Title.Value);
        result.IsChecked.ShouldBe(activity.IsChecked.Value);
        result.ParentRuleId.ShouldBeNull();
    }
    
    [Fact]
    public async Task GetActivityById_GivenNotExistingActivity_ShouldReturn204NoContentStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"daily-productive/activities/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}