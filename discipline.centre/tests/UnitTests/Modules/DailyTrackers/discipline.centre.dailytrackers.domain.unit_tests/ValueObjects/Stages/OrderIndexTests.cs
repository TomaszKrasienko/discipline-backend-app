using discipline.centre.dailytrackers.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ValueObjects.Stages;

public sealed class OrderIndexTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnOrderIndexWithValue()
    {
        //arrange
        var value = 1;
        
        //act
        var result = OrderIndex.Create(value);
        
        //assert
        result.Value.ShouldBe(value);
    }
    
    [Fact]
    public void Create_GivenIndexLessThan0_ShouldThrowDomainExceptionWithCodeDailyTrackerActivityStageIndexLessThanOne()
    {
        //act
        var exception = Record.Exception(() => OrderIndex.Create(0));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Stage.Index.LessThanOne");
    }
}