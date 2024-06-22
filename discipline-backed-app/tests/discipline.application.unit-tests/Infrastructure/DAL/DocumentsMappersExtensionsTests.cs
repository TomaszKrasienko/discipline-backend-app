using discipline.application.Domain.Entities;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.tests.shared.Documents;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Infrastructure.DAL;

public sealed class DocumentsMappersExtensionsTests
{
    [Fact]
    public void AsDocument_GivenActivityRuleWithoutSelectedDays_ShouldReturnActivityRuleDocumentWithNullSelectedDays()
    {
        //arrange
        var activityRule = ActivityRuleFactory.Get();
        
        //act
        var result = activityRule.AsDocument();
        
        //assert
        result.Id.ShouldBe(activityRule.Id.Value);
        result.Title.ShouldBe(activityRule.Title.Value);
        result.Mode.ShouldBe(activityRule.Mode.Value);
        result.SelectedDays.ShouldBeNull();
    }
    
    [Fact]
    public void AsDocument_GivenActivityRuleWithSelectedDays_ShouldReturnActivityRuleDocument()
    {
        //arrange
        List<int> selectedDays = [0, 1, 2];
        var activityRule = ActivityRuleFactory.Get(selectedDays);
        
        //act
        var result = activityRule.AsDocument();
        
        //assert
        result.Id.ShouldBe(activityRule.Id.Value);
        result.Title.ShouldBe(activityRule.Title.Value);
        result.Mode.ShouldBe(activityRule.Mode.Value);
        result.SelectedDays.Contains(selectedDays[0]).ShouldBeTrue();
        result.SelectedDays.Contains(selectedDays[1]).ShouldBeTrue();
        result.SelectedDays.Contains(selectedDays[2]).ShouldBeTrue();
    }

    [Fact]
    public void AsEntity_GivenActivityRuleDocumentWithoutSelectedDays_ShouldReturnActivityRuleWithEmptySelectedDays()
    {
        //arrange
        var activityRuleDocument = ActivityRuleDocumentFactory.Get();
        
        //act
        var result = activityRuleDocument.AsEntity();
        
        //assert
        result.Id.Value.ShouldBe(activityRuleDocument.Id);
        result.Title.Value.ShouldBe(activityRuleDocument.Title);
        result.Mode.Value.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays.ShouldBeEmpty();
    }
    
    [Fact]
    public void AsEntity_GivenActivityRuleDocumentWithSelectedDays_ShouldReturnActivityRule()
    {
        //arrange
        List<int> selectedDays = [0, 1, 2];
        var activityRuleDocument = ActivityRuleDocumentFactory.Get(selectedDays);
        
        //act
        var result = activityRuleDocument.AsEntity();
        
        //assert
        result.Id.Value.ShouldBe(activityRuleDocument.Id);
        result.Title.Value.ShouldBe(activityRuleDocument.Title);
        result.Mode.Value.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays.Select(x => x.Value).ToList().Contains(selectedDays[0]).ShouldBeTrue();
        result.SelectedDays.Select(x => x.Value).ToList().Contains(selectedDays[1]).ShouldBeTrue();
        result.SelectedDays.Select(x => x.Value).ToList().Contains(selectedDays[2]).ShouldBeTrue();
    }

    [Fact]
    public void AsDto_GivenActivityRuleDocumentWithoutSelectedDays_ShouldReturnActivityRuleDtoWithSelectedDaysAsNull()
    {
        //arrange
        var activityRuleDocument = ActivityRuleDocumentFactory.Get();
        
        //act
        var result = activityRuleDocument.AsDto();
        
        //assert
        result.Id.ShouldBe(activityRuleDocument.Id);
        result.Title.ShouldBe(activityRuleDocument.Title);
        result.Mode.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays.ShouldBeNull();
    }
    
    [Fact]
    public void AsDto_GivenActivityRuleDocumentWithSelectedDays_ShouldReturnActivityRuleDto()
    {
        //arrange
        List<int> selectedDays = [1, 4];
        var activityRuleDocument = ActivityRuleDocumentFactory.Get(selectedDays);
        
        //act
        var result = activityRuleDocument.AsDto();
        
        //assert
        result.Id.ShouldBe(activityRuleDocument.Id);
        result.Title.ShouldBe(activityRuleDocument.Title);
        result.Mode.ShouldBe(activityRuleDocument.Mode);
        result.SelectedDays.Contains(selectedDays[0]).ShouldBeTrue();
        result.SelectedDays.Contains(selectedDays[1]).ShouldBeTrue();
    }

    [Fact]
    public void AsDocument_GivenActivity_ShouldReturnActivityDocument()
    {
        //arrange
        var activity = ActivityFactory.Get();
        
        //act
        var result = activity.AsDocument();
        
        //assert
        result.Id.ShouldBe(activity.Id.Value);
        result.Title.ShouldBe(activity.Title.Value);
        result.IsChecked.ShouldBe(activity.IsChecked.Value);
        result.ParentRuleId.ShouldBeNull();
    }
}