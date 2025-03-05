using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ActivityTests;

public sealed class ClearParentRuleIdTests
{
    [Fact]
    public void GivenActivity_WhenClearParentActivityRuleIdIsCalled_ThenShouldChangeParentRuleIdToNull()
    {
        // Arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test", null),
            ActivityRuleId.New(), []);
        
        // Act
        activity.ClearParentActivityRuleId();
        
        // Assert
        activity.ParentActivityRuleId.ShouldBeNull();
    }
}