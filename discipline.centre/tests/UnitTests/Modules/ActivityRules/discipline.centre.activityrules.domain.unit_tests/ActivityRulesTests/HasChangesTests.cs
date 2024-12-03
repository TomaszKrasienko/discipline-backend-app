using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class HasChangesTests
{
    [Theory]
    [MemberData(nameof(GetHasChangesChangedParameters))]
    public void GivenAtLeastOneChangedParameter_ShouldReturnTrue(ActivityRule activityRule, HasChangesParameters parameters)
    {
        //act
        var result = activityRule.HasChanges(parameters.Title, parameters.Note, parameters.Mode, parameters.SelectedDays);
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Theory]
    [MemberData(nameof(GetHasChangesUnchangedParameters))]
    public void GivenAllUnchangedParameters_ShouldReturnFalse(ActivityRule activityRule, HasChangesParameters parameters)
    {
        //act
        var result = activityRule.HasChanges(parameters.Title, parameters.Note, parameters.Mode, parameters.SelectedDays);
        
        //assert
        result.ShouldBeFalse();
    }
}
