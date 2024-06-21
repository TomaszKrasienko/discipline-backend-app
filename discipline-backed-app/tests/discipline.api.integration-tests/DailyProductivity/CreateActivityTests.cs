using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.DailyProductivities;
using discipline.tests.shared.Entities;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.DailyProductivity;

[Collection("integration-tests")]
public sealed class CreateActivityTests : BaseTestsController
{
    [Fact]
    public async Task Create_GivenForFirstDailyActivity_ShouldReturn201CreatedStatusCodeAndAddDailyProductivityWithActivity()
    {
        //arrange
        var command = new CreateActivityCommand(Guid.Empty, "Test title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productive/current/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var isActivityExists = await DbContext
            .Activities
            .AsNoTracking()
            .AnyAsync(x => x.Id.Equals(resourceId));
        
        isActivityExists.ShouldBeTrue();

        var isDailyDisciplineExists = await DbContext
            .DailyProductivity
            .AnyAsync(x => x.Day == DateTime.Now.Date);
        
        isDailyDisciplineExists.ShouldBeTrue();
    }
    
    [Fact]
    public async Task Create_GivenForExistingDailyActivity_ShouldReturn201CreatedStatusCodeAndAddActivity()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        await DbContext.DailyProductivity.AddAsync(dailyProductivity);
        await DbContext.SaveChangesAsync();
        var command = new CreateActivityCommand(Guid.Empty, "Test title");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/daily-productive/current/add-activity", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var isActivityExists = await DbContext
            .Activities
            .AsNoTracking()
            .AnyAsync(x => x.Id.Equals(resourceId));
        
        isActivityExists.ShouldBeTrue();
    }
    
    [Fact]
    public async Task Create_GivenInvalidActivity_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        await DbContext.DailyProductivity.AddAsync(dailyProductivity);
        await DbContext.SaveChangesAsync();
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