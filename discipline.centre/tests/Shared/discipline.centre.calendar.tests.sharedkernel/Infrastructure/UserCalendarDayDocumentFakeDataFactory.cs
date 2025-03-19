using Bogus;
using discipline.centre.calendar.infrastructure.DAL.Calendar.Documents;
using discipline.centre.calendar.infrastructure.DAL.Documents;

namespace discipline.centre.calendar.tests.sharedkernel.Infrastructure;

public static class UserCalendarDayDocumentFakeDataFactory
{
    internal static UserCalendarDayDocument Get()
        => new Faker<UserCalendarDayDocument>()
            .RuleFor(f => f.UserCalendarId, Ulid.NewUlid().ToString())
            .RuleFor(f => f.UserId, Ulid.NewUlid().ToString())
            .RuleFor(f => f.Day, v => DateOnly.FromDateTime(v.Date.Recent()));

    internal static UserCalendarDayDocument AddTimeEventDocument(this UserCalendarDayDocument document, 
        bool description = false,
        bool timeTo = false)
    {
        var faker = new Faker();
        var events = document.Events?.ToList() ?? [];
        var from = new TimeOnly(faker.Random.Int(min: 1, max: 10)); 
        events.Add(new TimeEventDocument()
        {
            EventId = Ulid.NewUlid().ToString(),
            Content = new CalendarEventContentDocument()
            {
                Title = faker.Random.Word(),
                Description = description ? faker.Lorem.Sentence() : null,
            },
            TimeSpan = new TimeEventTimeSpanDocument()
            {
                From = from,
                To = timeTo ? from.AddHours(1) : null
            }
        });
        
        return document with { Events = events };
    }
    
    internal static UserCalendarDayDocument AddImportantDayDocument(this UserCalendarDayDocument document, 
        bool description = false)
    {
        var faker = new Faker();
        var events = document.Events?.ToList() ?? [];
        events.Add(new ImportantDateEventDocument()
        {
            EventId = Ulid.NewUlid().ToString(),
            Content = new CalendarEventContentDocument()
            {
                Title = faker.Random.Word(),
                Description = description ? faker.Lorem.Sentence() : null,
            }
        });
        
        return document with { Events = events };
    }
}