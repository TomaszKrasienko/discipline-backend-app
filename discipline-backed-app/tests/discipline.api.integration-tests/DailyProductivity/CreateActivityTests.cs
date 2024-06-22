using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.Entities;
using discipline.application.Features.DailyProductivities;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Repositories;
using discipline.tests.shared.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.DailyProductivity;

[Collection("integration-tests")]
public sealed class CreateActivityTests : BaseTestsController
{
    [Fact]
    public async Task Create_GivenForFirstDailyActivity_ShouldReturn200OkStatusCodeAndAddDailyProductivityWithActivity()
    {
        //arrange
        var command = new CreateActivityCommand(Guid.Empty, "Test title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productive/current/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var dailyProductivityDocument = await TestAppDb
            .GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
            .Find(x => x.Day == DateTime.Now.Date)
            .FirstOrDefaultAsync();

        dailyProductivityDocument.ShouldNotBeNull();
        dailyProductivityDocument.Activities.Any(x => x.Title == command.Title).ShouldBeTrue();
    }
    
    [Fact]
    public async Task Create_GivenForExistingDailyActivity_ShouldReturn200OkStatusCodeAndAddActivity()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        await TestAppDb.GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
            .InsertOneAsync(dailyProductivity.AsDocument());
        var command = new CreateActivityCommand(Guid.Empty, "Test title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productive/current/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
   
        var dailyProductivityDocument = await TestAppDb
            .GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
            .Find(x => x.Day == DateTime.Now.Date)
            .FirstOrDefaultAsync();

        dailyProductivityDocument.ShouldNotBeNull();
        dailyProductivityDocument.Activities.Any(x => x.Title == command.Title).ShouldBeTrue();
    }
    
    [Fact]
    public async Task Create_GivenInvalidActivity_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        await TestAppDb.GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
            .InsertOneAsync(dailyProductivity.AsDocument());
        var command = new CreateActivityCommand(Guid.Empty, activity.Title);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productive/current/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Create_GivenEmptyTitle_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new CreateActivityCommand(Guid.Empty, string.Empty);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productive/current/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}