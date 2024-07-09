using discipline.application.Domain.ActivityRules.Exceptions;
using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Domain.UsersCalendars.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.UsersCalendars.Entities.Create;

public class ImportantDateCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnEntityWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Test title";
        var eventDay = new DateOnly(2024, 1, 1);
        
        //act
        var entity = ImportantDate.Create(id, title, eventDay);
        
        //assert
        entity.Id.Value.ShouldBe(id);
        entity.Title.Value.ShouldBe(title);
        entity.EventDay.Value.ShouldBe(eventDay);
    }
    
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