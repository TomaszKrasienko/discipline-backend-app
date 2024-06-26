using System.Net;
using discipline.api.integration_tests._Helpers;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Repositories;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.DailyProductivity;

[Collection("integration-tests")]
public sealed class ChangeActivityCheckTests : BaseTestsController
{
    [Fact]
    public async Task ChangeActivityCheck_GivenValidActivityId_ShouldReturn200OkStatusCodeAndDeleteActivity()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        var isChecked = activity.IsChecked.Value;
        await TestAppDb.GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
            .InsertOneAsync(dailyProductivity.AsDocument());
        
        //act
        var response = await HttpClient.PatchAsync($"daily-productivity/activity/{activity.Id.Value}/change-check", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
                
        var dailyProductivityDocument = await TestAppDb
            .GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
            .Find(x => x.Day == DateOnly.FromDateTime(DateTime.Now.Date))
            .FirstOrDefaultAsync();
        dailyProductivityDocument.Activities
            .Any(x 
                => x.Id.Equals(activity.Id)
                && x.IsChecked == !isChecked).ShouldBeTrue();
    }
    
    [Fact]
    public async Task ChangeActivityCheck_GivenNotExistingActivityId_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        await TestAppDb.GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
            .InsertOneAsync(dailyProductivity.AsDocument());
        
        //act
        var response = await HttpClient.PatchAsync($"daily-productivity/activity/{Guid.NewGuid()}/change-check", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ChangeActivityCheck_GiveInvalidActivityId_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //act
        var response = await HttpClient.PatchAsync($"daily-productivity/activity/{Guid.Empty}/change-check", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}