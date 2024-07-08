using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Repositories;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.DailyProductivities;

[Collection("integration-tests")]
public class GetDailyActivityByDateTests : BaseTestsController
{
    [Fact]
    public async Task GetDailyActivityByDate_GivenExistingDailyProductivity_ShouldReturnDailyProductivityDto()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        await TestAppDb.GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
            .InsertOneAsync(dailyProductivity.AsDocument());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<DailyProductivityDto>($"/daily-productivity/{DateTime.Now:yyyy-MM-dd}");
        
        //assert
        result.Day.ShouldBe(DateOnly.FromDateTime(DateTime.Now));
        result.Activities.Any(x
            => x.Id.Equals(activity.Id)
            && x.Title == activity.Title
            && x.IsChecked == activity.IsChecked).ShouldBeTrue();
    }

    [Fact]
    public async Task GetDailyActivityByDay_GivenNotExistingDailyProductivity_ShouldReturn204NoContentStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"/daily-productivity/{DateTime.Now:yyyy-MM-dd}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}