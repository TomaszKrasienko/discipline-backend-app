using System.Net;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.integration_tests.shared;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.integration_tests;

[Collection("daily-trackers-module-mark-activity-as-checked")]
public sealed class MarkActivityAsCheckedTests() : BaseTestsController("daily-trackers-module")
{
    [Fact]
    public async Task GivenExistingActivity_WhenRequestedEndpoint_ThenShouldReturn204NoContentStatusCodeAndUpdateActivity()
    {
        // Arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var dailyTrackerId = DailyTrackerId.New();
        var activityId = ActivityId.New();

        var dailyTracker = DailyTracker.Create(dailyTrackerId, DateOnly.FromDateTime(DateTime.UtcNow.Date),
            user.Id, activityId, new ActivityDetailsSpecification("test_title", null), null,
            null);
        
        await TestAppDb.GetCollection<DailyTrackerDocument>().InsertOneAsync(dailyTracker.AsDocument());
                
        // Act
        var response = await HttpClient.PatchAsync($"api/daily-trackers-module/daily-trackers/{dailyTracker.Id.Value}/activities/{activityId.Value}/check", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GivenNonExistingActivity_WhenRequestedEndpoint_ThenShouldReturn400BadRequestStatusCode()
    {
        // Arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var dailyTrackerId = DailyTrackerId.New();
        var activityId1 = ActivityId.New();
        var activityId2 = ActivityId.New();

        var dailyTracker = DailyTracker.Create(dailyTrackerId, DateOnly.FromDateTime(DateTime.UtcNow.Date),
            user.Id, activityId1, new ActivityDetailsSpecification("test_title", null), null,
            null);
        
        await TestAppDb.GetCollection<DailyTrackerDocument>().InsertOneAsync(dailyTracker.AsDocument());
                
        // Act
        var response = await HttpClient.PatchAsync($"api/daily-trackers-module/daily-trackers/{dailyTracker.Id.Value}/activities/{activityId2.Value}/check", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GivenUnauthorizedUser_WhenRequestedEndpoint_ThenShouldReturn401UnauthorizedStatusCode()
    {
        // Arrange
        var dailyTrackerId = DailyTrackerId.New();
        var activityId = ActivityId.New();
                
        // Act
        var response = await HttpClient.PatchAsync($"api/daily-trackers-module/daily-trackers/{dailyTrackerId.Value}/activities/{activityId}/check", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GivenUserWithoutSubscription_WhenRequestedEndpoint_ThenShouldReturn401UnauthorizedStatusCode()
    {
        // Arrange
        _ = await AuthorizeWithoutSubscription();
        var dailyTrackerId = DailyTrackerId.New();
        var activityId = ActivityId.New();
                
        // Act
        var response = await HttpClient.PatchAsync($"api/daily-trackers-module/daily-trackers/{dailyTrackerId.Value}/activities/{activityId}/check", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}