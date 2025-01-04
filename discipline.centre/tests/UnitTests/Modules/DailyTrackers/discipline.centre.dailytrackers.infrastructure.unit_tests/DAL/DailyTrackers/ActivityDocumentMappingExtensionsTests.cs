using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.dailytrackers.sharedkernel.Domain;
using discipline.centre.dailytrackers.sharedkernel.Infrastructure;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using StackExchange.Redis;
using Xunit;

namespace discipline.centre.dailytrackers.infrastructure.unit_tests.DAL.DailyTrackers;

public sealed class ActivityDocumentMappingExtensionsTests
{
    [Fact]
    public void MapAsDto_WhenActivityDocumentWithoutStages_ShouldReturnActivityDtoWithoutStages()
    {
        //arrange
        var activityDocument = ActivityDocumentFakeDataFactory.Get(true, true, null);
        
        //act
        var result = activityDocument.MapAsDto();

        //assert
        result.ActivityId.ShouldBe(ActivityId.Parse(activityDocument.ActivityId));
        result.Details.Title.ShouldBe(activityDocument.Title);
        result.Details.Note.ShouldBe(activityDocument.Note);
        result.IsChecked.ShouldBe(activityDocument.IsChecked);
        result.ParentActivityRuleId.ShouldBe(ActivityRuleId.Parse(activityDocument.ParentActivityRuleId!));
        result.Stages.ShouldBeNull();
    }
    
    [Fact]
    public void MapAsDto_WhenActivityDocumentWithStages_ShouldReturnActivityDtoWithStages()
    {
        //arrange
        var stageDocument = StageDocumentFakeDataFactory.Get(1);
        var activityDocument = ActivityDocumentFakeDataFactory.Get(true, true, [stageDocument]);
        
        //act
        var result = activityDocument.MapAsDto();

        //assert
        result.ActivityId.ShouldBe(ActivityId.Parse(activityDocument.ActivityId));
        result.Details.Title.ShouldBe(activityDocument.Title);
        result.Details.Note.ShouldBe(activityDocument.Note);
        result.IsChecked.ShouldBe(activityDocument.IsChecked);
        result.ParentActivityRuleId.ShouldBe(ActivityRuleId.Parse(activityDocument.ParentActivityRuleId!));
        result.Stages!.Count.ShouldBe(1);
        result.Stages.First().Title.ShouldBe(stageDocument.Title);
        result.Stages.First().Index.ShouldBe(stageDocument.Index);
        result.Stages.First().IsChecked.ShouldBe(stageDocument.IsChecked);
    }
}