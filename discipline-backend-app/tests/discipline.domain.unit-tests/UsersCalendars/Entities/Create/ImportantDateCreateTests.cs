using discipline.domain.ActivityRules.Exceptions;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.UsersCalendars.Entities.Create;

public class ImportantDateCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnEntityWithFilledFields()
    {
        //arrange
        var id = EventId.New();
        var title = "Test title";
        
        //act
        var entity = ImportantDate.Create(id, title);
        
        //assert
        entity.Id.ShouldBe(id);
        entity.Title.Value.ShouldBe(title);
    }
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyEventTitleException()
    {
        //act
        var exception = Record.Exception(() => ImportantDate.Create(EventId.New(),
            string.Empty));
        
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
        var exception = Record.Exception(() => ImportantDate.Create(EventId.New(),
            title));
        
        //arrange
        exception.ShouldBeOfType<InvalidActivityRuleTitleLengthException>();
    }
}