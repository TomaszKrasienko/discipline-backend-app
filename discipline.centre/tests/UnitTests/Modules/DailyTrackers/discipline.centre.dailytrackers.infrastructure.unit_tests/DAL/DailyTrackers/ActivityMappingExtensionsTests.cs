using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.infrastructure.unit_tests.DAL.DailyTrackers;

public sealed class ActivityMappingExtensionsTests
{
    [Fact]
    public void AsDocument_WhenActivityWithoutStages_ShouldReturnActivityDocumentWithNullStages()
    {
        //arrange
        var activity = ActivityFakeDataFactory.Get(true, true, null);
        
        //act
        var document = activity.AsDocument();
        
        //assert
        document.ActivityId.ShouldBe(activity.Id.Value.ToString());
        document.Title.ShouldBe(activity.Details.Title);
        document.Note.ShouldBe(activity.Details.Note);
        document.IsChecked.ShouldBe(activity.IsChecked.Value);
        document.ParentActivityRuleId.ShouldBe(activity.ParentActivityRuleId!.Value.ToString());
        document.Stages.ShouldBeNull();
    } 
    
    [Fact]
    public void AsDocument_WhenActivityWithStages_ShouldReturnActivityDocumentWithStages()
    {
        //arrange
        var stage = StageFakeDataFactory.Get(1);
        var activity = ActivityFakeDataFactory.Get(true, true, [stage]);
        
        //act
        var document = activity.AsDocument();
        
        //assert
        document.ActivityId.ShouldBe(activity.Id.Value.ToString());
        document.Title.ShouldBe(activity.Details.Title);
        document.Note.ShouldBe(activity.Details.Note);
        document.IsChecked.ShouldBe(activity.IsChecked.Value);
        document.ParentActivityRuleId.ShouldBe(activity.ParentActivityRuleId!.Value.ToString());
        document.Stages!.First().Title.ShouldBe(stage.Title);
        document.Stages!.First().Index.ShouldBe(stage.Index.Value);
        document.Stages!.First().IsChecked.ShouldBe(stage.IsChecked.Value);
    } 
}