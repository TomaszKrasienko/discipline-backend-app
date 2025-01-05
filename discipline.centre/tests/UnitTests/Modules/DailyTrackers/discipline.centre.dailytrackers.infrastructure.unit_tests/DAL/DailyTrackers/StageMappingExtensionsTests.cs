using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.dailytrackers.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.infrastructure.unit_tests.DAL.DailyTrackers;

public sealed class StageMappingExtensionsTests
{
    [Fact]
    public void MapAsDocument_GivenDailyTracker_ShouldReturnDailyTrackerDocument()
    {
        //arrange
        var stage = StageFakeDataFactory.Get(1);
        var activity = ActivityFakeDataFactory.Get(true, true, [stage]);
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);
        
        //act
        var document = dailyTracker.MapAsDocument();
        
        //assert
        document.DailyTrackerId.ShouldBe(dailyTracker.Id.ToString());
        document.UserId.ShouldBe(dailyTracker.UserId.ToString());
        document.Day.ShouldBe(document.Day);
        document.Activities.First().Title.ShouldBe(activity.Details.Title);
        document.Activities.First().Note.ShouldBe(activity.Details.Note);
        document.Activities.First().ParentActivityRuleId.ShouldBe(activity.ParentActivityRuleId?.ToString());
        document.Activities.First().IsChecked.ShouldBe(activity.IsChecked.Value);
        document.Activities.First().Stages?.First().Title.ShouldBe(stage.Title);
        document.Activities.First().Stages?.First().Index.ShouldBe(stage.Index.Value);
        document.Activities.First().Stages?.First().IsChecked.ShouldBeFalse();
    }
}