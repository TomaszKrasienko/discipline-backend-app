using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ActivityTests;

public sealed class MarkAsCheckedShould
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
}