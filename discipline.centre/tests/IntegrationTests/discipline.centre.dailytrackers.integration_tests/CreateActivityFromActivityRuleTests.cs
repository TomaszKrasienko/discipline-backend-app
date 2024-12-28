using System.Net;
using System.Net.Http.Json;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.integration_tests.shared;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.integration_tests;

[Collection("daily-trackers-module-create-activity-from-activity-rule")]
public sealed class CreateActivityFromActivityRuleTests() : BaseTestsController("daily-trackers-module")
{
    [Fact]
    public async Task GivenExistingActivityRuleAndNewDailyTrackerAndValidArguments_ShouldReturn201CreatedStatusCodeAndAddActivityToNewDailyTracker()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var activityRuleDocument = activityRule.MapAsDocument();
        await TestAppDb.GetCollection<ActivityRuleDocument>("activity-rules-module")
            .InsertOneAsync(activityRuleDocument with {UserId = user.Id.ToString()});

        var command = new CreateActivityFromActivityRuleCommand(activityRule.Id, user.Id);
        
        //act
        var result = await HttpClient.PostAsJsonAsync(
            $"api/daily-trackers-module/daily-trackers/activities/{activityRule.Id.ToString()}", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var resourceId = GetResourceIdFromHeader(result);
        var dailyTracker = await TestAppDb.GetCollection<DailyTrackerDocument>()
            .Find(x => x.Activities.Any(y => y.ActivityId == resourceId))
            .SingleOrDefaultAsync();
        
        dailyTracker.Activities.First().ParentActivityRuleId.ShouldBe(activityRule.Id.ToString());
        dailyTracker.Activities.First().Title.ShouldBe(activityRule.Details.Title);
        
    }
}