using discipline.centre.activityrules.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ValueObjects.Stages;

public sealed class OrderIndexTests
{
    [Fact]
    public void Create_GivenValidValue_ShouldReturnOrderIndexWithValue()
    {
        //arrange
        var value = 1;
        
        //act
        var result = OrderIndex.Create(value);
        
        //assert
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void Create_GivenValueBelowOne_ShouldReturnDomainExceptionWithCodeActivityRuleStageIndexLessThanOne()
    {
        //act
        var exception = Record.Exception(() => OrderIndex.Create(-1));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stage.Index.LessThanOne");
    }
}