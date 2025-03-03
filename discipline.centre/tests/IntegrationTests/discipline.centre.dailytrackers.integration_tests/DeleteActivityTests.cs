using System.Net;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.dailytrackers.tests.sharedkernel.Domain;
using discipline.centre.integration_tests.shared;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.integration_tests;

[Collection("daily-trackers-module-delete-activity")]
public sealed class DeleteActivityTests() : BaseTestsController("daily-trackers-module")
{
    [Fact]
    public async Task GivenExistingDailyTrackerWithMoreThanOneActivity_WhenRequestedEndpoint_ShouldReturn204NoContentStatusCodeAndDeleteActivity()
    {
        // Arrange 
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var activity = ActivityFakeDataFactory.Get();
        var dailyTracker = DailyTrackerFakeDataFactory.Get(null, user.Id);
        dailyTracker.AddActivity(activity.Id, new ActivityDetailsSpecification(activity.Details.Title, null),
            null, []);
        
        await TestAppDb.GetCollection<DailyTrackerDocument>()
            .InsertOneAsync(dailyTracker.AsDocument());
        
        // Act
        var response = await HttpClient.DeleteAsync($"$api/daily-trackers-module/daily-trackers/{dailyTracker.Id.ToString()}/activities/{activity.Id.ToString()}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
        var updatedDailyTracker = await TestAppDb.GetCollection<DailyTrackerDocument>()
            .Find(x => x.DailyTrackerId == dailyTracker.Id.ToString())
            .FirstOrDefaultAsync();

        updatedDailyTracker.ShouldNotBeNull();
        updatedDailyTracker.Activities.Count().ShouldBe(1);
    }
}