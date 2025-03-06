using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discpline.centre.calendar.domain.ValueObjects;
using Shouldly;

namespace discipline.centre.calendar.domain.unit_tests.ValueObjects;

public sealed class EventTimeSpanTests
{
    [Fact]
    public void GivenCorrectBothArguments_WhenCreate_ShouldReturnEventTimeSpan()
    {
        // Arrange
        var from = new TimeOnly(11, 00);
        var to = new TimeOnly(12, 00);
        
        // Act
        var result = EventTimeSpan.Create(from, to);
        
        // Assert
        result.From.ShouldBe(from);
        result.To.ShouldBe(to);
    }
    
    [Fact]
    public void GivenFrom_WhenCreate_ShouldReturnEventTimeSpan()
    {
        // Arrange
        var from = new TimeOnly(11, 00);
        
        // Act
        var result = EventTimeSpan.Create(from, null);
        
        // Assert
        result.From.ShouldBe(from);
        result.To.ShouldBeNull();
    }
    
    [Fact]
    public void GivenToValueBiggerThanFrom_WhenCreate_ShouldThrowDomainExceptionWithCodeTimeEventEventTimeSpanTooHighToValue()
    {
        // Act
        var exception = Record.Exception(() => EventTimeSpan.Create(new TimeOnly(11,00), new TimeOnly(10,00) ));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("TimeEvent.EventTimeSpan.TooHighToValue");
    }
}