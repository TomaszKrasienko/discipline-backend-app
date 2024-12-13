using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.dailytrackers.sharedkernel.Domain;
using discipline.centre.dailytrackers.sharedkernel.Infrastructure;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.infrastructure.unit_tests.DAL.DailyTrackers;

public sealed class DailyTrackerDocumentMappingExtensionsTests
{
    [Fact]
    public void MapAsEntity_GivenDailyTrackerDocument_ShouldMapToDailyTracker()
    {
        //arrange
        var stageDocument = StageDocumentFakeDataFactory.Get();
        var activityDocument = ActivityDocumentFakeDataFactory.Get(true, true, [stageDocument]);
        var dailyTrackerDocument = DailyTrackerDocumentFakeDataFactory.Get([activityDocument]);
        
        //act
        var entity = dailyTrackerDocument.MapAsDocument();

        //assert
        entity.Id.ShouldBe(DailyTrackerId.Parse(dailyTrackerDocument.DailyTrackerId));
        entity.UserId.ShouldBe(UserId.Parse(dailyTrackerDocument.UserId));
        entity.Day.Value.ShouldBe(dailyTrackerDocument.Day);
        entity.Activities.First().Id.ShouldBe(ActivityId.Parse(activityDocument.ActivityId));
        entity.Activities.First().Details.Title.ShouldBe(activityDocument.Title);
        entity.Activities.First().Details.Note.ShouldBe(activityDocument.Note);
        entity.Activities.First().IsChecked.Value.ShouldBe(activityDocument.IsChecked);
        entity.Activities.First().Stages![0].Id.ShouldBe(StageId.Parse(stageDocument.StageId));
        entity.Activities.First().Stages![0].Title.Value.ShouldBe(stageDocument.Title);
        entity.Activities.First().Stages![0].Index.Value.ShouldBe(stageDocument.Index);
        entity.Activities.First().Stages![0].IsChecked.Value.ShouldBe(stageDocument.IsChecked);
    }
}