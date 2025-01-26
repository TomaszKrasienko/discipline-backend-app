using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.dailytrackers.tests.sharedkernel.Infrastructure;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.infrastructure.unit_tests.DAL.DailyTrackers;

public sealed class DailyTrackerDocumentMappingExtensionsTests
{
    [Fact]
    public void AsEntity_GivenDailyTrackerDocument_ShouldMapToDailyTracker()
    {
        //arrange
        var stageDocument = StageDocumentFakeDataFactory.Get();
        var activityDocument = ActivityDocumentFakeDataFactory.Get(true, true, [stageDocument]);
        var dailyTrackerDocument = DailyTrackerDocumentFakeDataFactory.Get([activityDocument]);
        
        //act
        var entity = dailyTrackerDocument.AsEntity();

        //assert
        entity.Id.ShouldBe(DailyTrackerId.Parse(dailyTrackerDocument.DailyTrackerId));
        entity.UserId.ShouldBe(UserId.Parse(dailyTrackerDocument.UserId));
        entity.Day.Value.ShouldBe(dailyTrackerDocument.Day);
        entity.Activities.First().Id.ShouldBe(ActivityId.Parse(activityDocument.ActivityId));
        entity.Activities.First().Details.Title.ShouldBe(activityDocument.Title);
        entity.Activities.First().Details.Note.ShouldBe(activityDocument.Note);
        entity.Activities.First().IsChecked.Value.ShouldBe(activityDocument.IsChecked);
        entity.Activities.First().Stages!.First().Id.ShouldBe(StageId.Parse(stageDocument.StageId));
        entity.Activities.First().Stages!.First().Title.Value.ShouldBe(stageDocument.Title);
        entity.Activities.First().Stages!.First().Index.Value.ShouldBe(stageDocument.Index);
        entity.Activities.First().Stages!.First().IsChecked.Value.ShouldBe(stageDocument.IsChecked);
    }

    [Fact]
    public void AsDto_GivenDailyTrackerDocument_ShouldMapToDailyTrackerDto()
    {
        //arrange
        var stageDocument = StageDocumentFakeDataFactory.Get();
        var activityDocument = ActivityDocumentFakeDataFactory.Get(true, true, [stageDocument]);
        var dailyTrackerDocument = DailyTrackerDocumentFakeDataFactory.Get([activityDocument]);
        
        //act
        var dto = dailyTrackerDocument.AsDto();
        
        //assert
        dto.Day.ShouldBe(dailyTrackerDocument.Day);
        dto.Activities.First().ActivityId.Value.ToString().ShouldBe(activityDocument.ActivityId);
        dto.Activities.First().Details.Title.ShouldBe(activityDocument.Title);
        dto.Activities.First().Details.Note.ShouldBe(activityDocument.Note);
        dto.Activities.First().ParentActivityRuleId!.Value.ToString().ShouldBe(activityDocument.ParentActivityRuleId);
        dto.Activities.First().IsChecked.ShouldBe(activityDocument.IsChecked);
        dto.Activities.First().Stages!.First().StageId.Value.ToString().ShouldBe(stageDocument.StageId);
        dto.Activities.First().Stages!.First().Title.ShouldBe(stageDocument.Title);
        dto.Activities.First().Stages!.First().Index.ShouldBe(stageDocument.Index);
        dto.Activities.First().Stages!.First().IsChecked.ShouldBe(stageDocument.IsChecked);
    }
}