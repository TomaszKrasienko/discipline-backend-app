using System.Net;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.integration_tests.shared;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.integration_tests;

[Collection("daily-trackers-module-mark-activity-stage-as-checked")]
public sealed class MarkActivityStageAsCheckedTests() : BaseTestsController("daily-trackers-module")
{
    [Fact]
    public async Task ShouldReturn204StatusCodeAndUpdateActivityStageInDb_WhenProvidedCorrectStageParameters()
    {
        // Arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var dailyTrackerId = DailyTrackerId.New();
        var activityId = ActivityId.New();

        var dailyTracker = DailyTracker.Create(dailyTrackerId, DateOnly.FromDateTime(DateTime.UtcNow.Date),
            user.Id, activityId, new ActivityDetailsSpecification("test_title", null), null,
            [new StageSpecification("test_title", 1)]);

        var stageId = dailyTracker.Activities.First().Stages!.First().Id;

        await TestAppDb.GetCollection<DailyTrackerDocument>().InsertOneAsync(dailyTracker.AsDocument());
        
        // Act
        var response = await HttpClient.PatchAsync($"api/daily-trackers-module/daily-trackers/{dailyTracker.Id.Value}/activities/{activityId.Value}/stages/{stageId.Value}/check", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ShouldReturn400StatusCode_WhenStageNotExistsForActivity()
    {
        // Arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var dailyTrackerId = DailyTrackerId.New();
        var activityId = ActivityId.New();

        var dailyTracker = DailyTracker.Create(dailyTrackerId, DateOnly.FromDateTime(DateTime.UtcNow.Date),
            user.Id, activityId, new ActivityDetailsSpecification("test_title", null), null,
            null);

        var stageId = StageId.New();

        await TestAppDb.GetCollection<DailyTrackerDocument>().InsertOneAsync(dailyTracker.AsDocument());
        
        // Act
        var response = await HttpClient.PatchAsync($"api/daily-trackers-module/daily-trackers/{dailyTracker.Id.Value}/activities/{activityId.Value}/stages/{stageId.Value}/check", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ShouldReturn401UnauthorizedStatusCode_WhenUserUnathorized()
    {
        // Arrange
        var dailyTrackerId = DailyTrackerId.New();
        var activityId = ActivityId.New();
        var stageId = StageId.New();
        
        // Act
        var response = await HttpClient.PatchAsync($"api/daily-trackers-module/daily-trackers/{dailyTrackerId.Value}/activities/{activityId.Value}/stages/{stageId.Value}/check", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task ShouldReturn403ForbiddenStatusCode_WhenSAuthorizedWithoutSubscription()
    {
        // Arrange
        _ = AuthorizeWithoutSubscription();
        var dailyTrackerId = DailyTrackerId.New();
        var activityId = ActivityId.New();
        var stageId = StageId.New();
        
        // Act
        var response = await HttpClient.PatchAsync($"api/daily-trackers-module/daily-trackers/{dailyTrackerId.Value}/activities/{activityId.Value}/stages/{stageId.Value}/check", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task ShouldReturn404NotFoundStatusCode_WhenDailyTrackerNotFound()
    {
        // Arrange
        _ = await AuthorizeWithFreeSubscriptionPicked();
        var dailyTrackerId = DailyTrackerId.New();
        var activityId = ActivityId.New();
        var stageId = StageId.New();
        
        // Act
        var response = await HttpClient.PatchAsync($"api/daily-trackers-module/daily-trackers/{dailyTrackerId.Value}/activities/{activityId.Value}/stages/{stageId.Value}/check", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}