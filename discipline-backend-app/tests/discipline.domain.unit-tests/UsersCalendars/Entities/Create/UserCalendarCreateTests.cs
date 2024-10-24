using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.UsersCalendars.Entities.Create;

public sealed class UserCalendarCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnUserCalendar()
    {
        //arrange
        var day = new DateOnly(2024, 1, 1);
        var userId = UserId.New();
        var id = UserCalendarId.New();
        
        //act
        var result = UserCalendar.Create(id, day, userId);
        
        //assert
        result.Id.ShouldBe(id);
        result.Day.Value.ShouldBe(day);
        result.UserId.ShouldBe(userId);
    }
}