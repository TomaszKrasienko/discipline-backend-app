using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.StageTests;

public sealed class MarkAsCheckedTests
{
    [Fact]
    public void MarkAsChecked_Always_SetsIsCheckedToTrue()
    {
        //arrange
        var stage = Stage.Create(StageId.New(), "test_stage_title", 1);
        
        //act
        stage.MarkAsChecked();
        
        //assert
        stage.IsChecked.Value.ShouldBeTrue();
    }
}