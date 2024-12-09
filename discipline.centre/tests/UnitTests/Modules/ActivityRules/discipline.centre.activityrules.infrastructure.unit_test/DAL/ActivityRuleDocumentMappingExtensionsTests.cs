using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.tests.sharedkernel.Infrastructure;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace dicipline.centre.activityrules.infrastructure.unit_tests.DAL;

public sealed class ActivityRuleDocumentMappingExtensionsTests
{
    [Fact]
    public void AsEntity_GivenActivityRuleDocumentWithoutSelectedDays_ShouldReturnActivityRuleWithEmptySelectedDays()
    {
        //arrange
        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get();
        
        //act
        var result = activityRuleDocument.MapAsEntity();
        
        //assert
        result.Id.ShouldBe(ActivityRuleId.Parse(activityRuleDocument.Id));
        result.UserId.ShouldBe(UserId.Parse(activityRuleDocument.UserId));
        result.Details.Title.ShouldBe(activityRuleDocument.Title);
        result.Details.Note.ShouldBe(activityRuleDocument.Note);
        result.Mode.Value.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays.ShouldBeNull();
        result.Stages.ShouldBeNull();
    }
    
    [Fact]
    public void AsEntity_GivenActivityRuleDocumentWithSelectedDays_ShouldReturnActivityRule()
    {
        //arrange
        List<int> selectedDays = [0, 1, 2];
        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get(selectedDays);
        var stageDocument = StageDocumentFakeDataFactory.Get();
        activityRuleDocument = new()
        {
            Id = activityRuleDocument.Id,
            UserId = activityRuleDocument.UserId,
            Title = activityRuleDocument.Title,
            Note = activityRuleDocument.Note,
            Mode = activityRuleDocument.Mode,
            SelectedDays = activityRuleDocument.SelectedDays,
            Stages = [stageDocument]
        };
        
        //act
        var result = activityRuleDocument.MapAsEntity();
        
        //assert
        result.Id.ShouldBe(ActivityRuleId.Parse(activityRuleDocument.Id));
        result.UserId.ShouldBe(UserId.Parse(activityRuleDocument.UserId));
        result.Details.Title.ShouldBe(activityRuleDocument.Title);
        result.Details.Note.ShouldBe(activityRuleDocument.Note);
        result.Mode.Value.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays!.Values.Select(x => (int)x).SequenceEqual(selectedDays).ShouldBeTrue();
        result.Stages![0].Id.ShouldBe(StageId.Parse(stageDocument.StageId));
        result.Stages![0].Title.Value.ShouldBe(stageDocument.Title);
        result.Stages![0].Index.Value.ShouldBe(stageDocument.Index);
    }
    
    [Fact]
    public void MapAsDto_GivenActivityRuleDocumentWithoutSelectedDays_ShouldReturnActivityRuleDtoWithSelectedDaysAsNull()
    {
        //arrange
        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get();
        
        //act
        var result = activityRuleDocument.MapAsDto();
        
        //assert
        result.ActivityRuleIdId.ShouldBe(ActivityRuleId.Parse(activityRuleDocument.Id));
        result.Title.ShouldBe(activityRuleDocument.Title);
        result.Note.ShouldBe(activityRuleDocument.Note);
        result.Mode.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays.ShouldBeNull();
    }
    
    [Fact]
    public void MapAsDto_GivenActivityRuleDocumentWithSelectedDays_ShouldReturnActivityRuleDto()
    {
        //arrange
        List<int> selectedDays = [1, 4];
        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get(selectedDays);
        var stageDocument = StageDocumentFakeDataFactory.Get();
        activityRuleDocument = new()
        {
            Id = activityRuleDocument.Id,
            UserId = activityRuleDocument.UserId,
            Title = activityRuleDocument.Title,
            Note = activityRuleDocument.Note,
            Mode = activityRuleDocument.Mode,
            SelectedDays = activityRuleDocument.SelectedDays,
            Stages = [stageDocument]
        };
        
        //act
        var result = activityRuleDocument.MapAsDto();
        
        //assert
        result.ActivityRuleIdId.ShouldBe(ActivityRuleId.Parse(activityRuleDocument.Id));
        result.Title.ShouldBe(activityRuleDocument.Title);
        result.Mode.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays!.Contains(selectedDays[0]).ShouldBeTrue();
        result.SelectedDays!.Contains(selectedDays[1]).ShouldBeTrue();
        result.Stages![0].StageId.ShouldBe(StageId.Parse(stageDocument.StageId));
        result.Stages![0].Title.ShouldBe(stageDocument.Title);
        result.Stages![0].Index.ShouldBe(stageDocument.Index);
    }
}