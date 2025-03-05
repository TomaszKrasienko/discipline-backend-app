using discipline.centre.dailytrackers.domain.ValueObjects.DailyTrackers;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ValueObjects.DailyTrackers;

public sealed class DayTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnDayWithValue()
    {
        //arrange
        var value = DateOnly.FromDateTime(DateTime.Now);
        
        //act
        var result = Day.Create(value);
        
        //assert
        result.Value.ShouldBe(value);
    }
    
    [Fact]
    public void Create_GivenDefaultDateOnly_ShouldThrowDomainExceptionWithCode()
    {
        //act
        var exception = Record.Exception(() => Day.Create(default));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Day.Default");
    }
}