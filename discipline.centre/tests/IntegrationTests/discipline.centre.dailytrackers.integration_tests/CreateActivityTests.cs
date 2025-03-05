using System.Net;
using System.Net.Http.Json;
using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.integration_tests.shared;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.integration_tests;

[Collection("daily-trackers-module-create-activity")]
public sealed class CreateActivityTests() : BaseTestsController("daily-trackers-module")
{
    [Fact]
    public async Task GivenValidArguments_ShouldReturn200OkStatusCodeAndAddActivity()
    {
        //arrange 
        _ = await AuthorizeWithFreeSubscriptionPicked();

        var createActivityDto = new CreateActivityDto(DateOnly.FromDateTime(DateTime.Now),
            new ActivityDetailsSpecification("test_activity_title", "test_activity_note"),
            [new StageSpecification("test_stage_title", 1)]);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("api/daily-trackers-module/daily-trackers/activities", createActivityDto);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var identifier = GetResourceIdFromHeader(response);
        
        var isActivityExists = await TestAppDb.GetCollection<DailyTrackerDocument>()
            .Find(x => x.Activities.Any(y => y.ActivityId.ToString() == identifier))
            .AnyAsync();
        
        isActivityExists.ShouldBeTrue();
    }
    
    [Fact]
    public async Task GivenAlreadyExistingTitleForUserAndTheSameDay_ShouldReturn400BadRequest()
    {
        //arrange 
        var user = await AuthorizeWithFreeSubscriptionPicked();

        var createActivityDto = new CreateActivityDto(DateOnly.FromDateTime(DateTime.Now),
            new ActivityDetailsSpecification("test_activity_title", "test_activity_note"), null);
        
        var dailyTracker = new DailyTrackerDocument
        {
            DailyTrackerId = Ulid.NewUlid().ToString(),
            Day = createActivityDto.Day,
            UserId = user.Id.ToString(),
            Activities = [new ActivityDocument
            {
                ActivityId = Ulid.NewUlid().ToString(),
                IsChecked = false,
                Title = createActivityDto.Details.Title,
            }]
        };
        
        await TestAppDb.GetCollection<DailyTrackerDocument>().InsertOneAsync(dailyTracker);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("api/daily-trackers-module/daily-trackers/activities", createActivityDto);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange 
        var createActivityDto = new CreateActivityDto(DateOnly.FromDateTime(DateTime.Now),
            new ActivityDetailsSpecification("test_activity_title", "test_activity_note"),
            [new StageSpecification("test_stage_title", 1)]);

        //act
        var response = await HttpClient.PostAsJsonAsync("api/daily-trackers-module/daily-trackers/activities", createActivityDto);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task UAuthorizedByUserWithStatusCreated_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange 
        _ = await AuthorizeWithoutSubscription();
        
        var createActivityDto = new CreateActivityDto(DateOnly.FromDateTime(DateTime.Now),
            new ActivityDetailsSpecification("test_activity_title", "test_activity_note"),
            [new StageSpecification("test_stage_title", 1)]);

        //act
        var response = await HttpClient.PostAsJsonAsync("api/daily-trackers-module/daily-trackers/activities", createActivityDto);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}