using discipline.application.Domain.Entities;
using discipline.application.Domain.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Entities.UserCalendar;

public class ImportantDateCreateTests
{
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyEventTitleException()
    {
        //act
        var exception = Record.Exception(() => ImportantDate.Create(Guid.NewGuid(),
            string.Empty, new DateOnly(2024, 1, 1)));
        
        //arrange
        exception.ShouldBeOfType<EmptyEventTitleException>();
    }
    
    [Theory]
    [InlineData('t', 1)]
    [InlineData('t',110)]
    public void Create_GivenInvalidLengthTitle_ShouldThrowInvalidActivityRuleTitleLengthException(char text, int multiplier)
    {
        //arrange
        string title = new string(text, multiplier);
        
        //act
        var exception = Record.Exception(() => ImportantDate.Create(Guid.NewGuid(),
            title, new DateOnly(2024, 1, 1)));
        
        //arrange
        exception.ShouldBeOfType<InvalidActivityRuleTitleLengthException>();
    }
}