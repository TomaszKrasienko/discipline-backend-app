using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ActivityTests;

public sealed class MarkAsCheckedTests
{
    [Fact]
    public void ChangeIsCheckedAsTrue_Always()
    {
        // Arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test", null),
            null, null);
        
        // Act
        activity.MarkAsChecked();
        
        // Assert
        activity.IsChecked.Value.ShouldBeTrue();
    }

    [Fact]
    public void GivenActivityWithNotCheckedStages_WhenMarkAsChecked_ThenShouldMarkActivityAndStagesAsChecked()
    {
        // Arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_tite", null),
            null, [new StageSpecification("test_stage_title", 1)]);
        
        // Act
        activity.MarkAsChecked();
        
        // Assert
        activity.IsChecked.Value.ShouldBeTrue();
        activity.Stages!.First().IsChecked.Value.ShouldBeTrue();
    }
}