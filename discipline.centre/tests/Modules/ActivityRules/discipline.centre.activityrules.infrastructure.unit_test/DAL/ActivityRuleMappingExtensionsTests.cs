using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace dicipline.centre.activityrules.infrastructure.unit_tests.DAL;

public sealed class ActivityRuleMappingExtensionsTests
{
    [Fact]
    public void AsDocument_GivenActivityRuleWithoutSelectedDays_ShouldReturnActivityRuleDocumentWithNullSelectedDays()
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();
        
        //act
        var result = activityRule.MapAsDocument();
        
        //assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.UserId.ShouldBe(activityRule.UserId.ToString());
        result.Title.ShouldBe(activityRule.Title.Value);
        result.Mode.ShouldBe(activityRule.Mode.Value);
        result.SelectedDays.ShouldBeNull();
    }
    
    [Fact]
    public void AsDocument_GivenActivityRuleWithSelectedDays_ShouldReturnActivityRuleDocument()
    {
        //arrange
        List<int> selectedDays = [0, 1, 2];
        var activityRule = ActivityRuleFakeDateFactory.Get(selectedDays);
        
        //act
        var result = activityRule.MapAsDocument();
        
        //assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.UserId.ShouldBe(activityRule.UserId.ToString());
        result.Mode.ShouldBe(activityRule.Mode.Value);
        result.Title.ShouldBe(activityRule.Title.Value);
        result.SelectedDays!.Contains(selectedDays[0]).ShouldBeTrue();
        result.SelectedDays!.Contains(selectedDays[1]).ShouldBeTrue();
        result.SelectedDays!.Contains(selectedDays[2]).ShouldBeTrue();
    }
}