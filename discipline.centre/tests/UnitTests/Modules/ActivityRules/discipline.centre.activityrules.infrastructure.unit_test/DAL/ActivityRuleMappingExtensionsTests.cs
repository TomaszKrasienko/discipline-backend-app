using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.infrastructure.unit_tests.DAL;

public sealed class ActivityRuleMappingExtensionsTests
{
    [Fact]
    public void AsDocument_GivenActivityRuleWithoutSelectedDays_ShouldReturnActivityRuleDocumentWithNullSelectedDays()
    {
        //arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        
        //act
        var result = activityRule.MapAsDocument();
        
        //assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.UserId.ShouldBe(activityRule.UserId.ToString());
        result.Title.ShouldBe(activityRule.Details.Title);
        result.Note.ShouldBe(activityRule.Details.Note);
        result.Mode.ShouldBe(activityRule.Mode.Value);
        result.SelectedDays.ShouldBeNull();
        result.Stages.ShouldBeNull();
    }
    
    [Fact]
    public void AsDocument_GivenActivityRuleWithSelectedDays_ShouldReturnActivityRuleDocument()
    {
        //arrange
        List<int> selectedDays = [0, 1, 2];
        var activityRule = ActivityRuleFakeDataFactory.Get(true, selectedDays);
        var stage = StageFakeDataFactory.Get(1);
        var newStage = activityRule.AddStage(new StageSpecification(stage.Title, stage.Index));
        
        //act
        var result = activityRule.MapAsDocument();
        
        //assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.UserId.ShouldBe(activityRule.UserId.ToString());
        result.Mode.ShouldBe(activityRule.Mode.Value);
        result.Title.ShouldBe(activityRule.Details.Title);
        result.Note.ShouldBe(activityRule.Details.Note);
        result.SelectedDays!.Contains(selectedDays[0]).ShouldBeTrue();
        result.SelectedDays!.Contains(selectedDays[1]).ShouldBeTrue();
        result.SelectedDays!.Contains(selectedDays[2]).ShouldBeTrue();
        result.Stages!.First().StageId.ShouldBe(newStage.Id.ToString());
        result.Stages!.First().Title.ShouldBe(stage.Title);
        result.Stages!.First().Index.ShouldBe(stage.Index.Value);
    }
}