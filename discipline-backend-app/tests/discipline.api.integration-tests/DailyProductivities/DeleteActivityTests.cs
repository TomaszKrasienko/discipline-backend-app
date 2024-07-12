using System.Net;
using discipline.api.integration_tests._Helpers;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Repositories;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.DailyProductivities;

[Collection("integration-tests")]
public sealed class DeleteActivityTests : BaseTestsController
{
    [Fact]
    public async Task Delete_GivenValidActivityId_ShouldReturn200OkStatusCodeAndDeleteActivity()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        await TestAppDb.GetCollection<DailyProductivityDocument>()
            .InsertOneAsync(dailyProductivity.AsDocument());
        
        //act
        var response = await HttpClient.DeleteAsync($"daily-productivity/activity/{activity.Id.Value}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var dailyProductivityDocument = await TestAppDb
            .GetCollection<DailyProductivityDocument>()
            .Find(x => x.Day == DateOnly.FromDateTime(DateTime.Now.Date))
            .FirstOrDefaultAsync();
        
        dailyProductivityDocument.Activities.Any(x => x.Id.Equals(activity.Id)).ShouldBeFalse();
    }
    
    [Fact]
    public async Task Delete_GivenNotExistingActivityId_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        await TestAppDb.GetCollection<DailyProductivityDocument>()
            .InsertOneAsync(dailyProductivity.AsDocument());
        
        //act
        var response = await HttpClient.DeleteAsync($"daily-productivity/activity/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}