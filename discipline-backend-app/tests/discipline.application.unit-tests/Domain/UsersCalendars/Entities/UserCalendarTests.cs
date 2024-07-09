using discipline.application.Domain.UsersCalendars.Entities;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.UsersCalendars.Entities;

public sealed class UserCalendarTests
{
    [Fact]
    public void AddEvent_GivenIdAndTitle_ShouldAddImportantDateToEvents()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var id = Guid.NewGuid();
        var title = "test_title";
        
        //act
        userCalendar.AddEvent(id, title);
        
        //assert
        var @event = userCalendar.Events.FirstOrDefault(x => x.Id.Equals(id));
        @event.ShouldBeOfType<ImportantDate>();
        @event.ShouldNotBeNull();
        @event.Title.Value.ShouldBe(title);
    }
    
}