using Bogus;
using discipline.centre.calendar.domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.calendar.tests.sharedkernel.Domain;

public static class UserCalendarDayFakeDataFactory
{
    public static UserCalendarDay GetWithImportantDate(bool description = false)
        => new Faker<UserCalendarDay>()
            .CustomInstantiator(v => UserCalendarDay.CreateWithImportantDate(
                UserCalendarId.New(),
                UserId.New(),
                DateOnly.FromDateTime(v.Date.Recent()),
                CalendarEventId.New(),
                v.Random.Word(),
                description ? v.Lorem.Sentence() : null)).Generate();

    public static UserCalendarDay AddTimeEvent(this UserCalendarDay userCalendarDay, 
        bool description = false,
        bool timeTo = false)
    {
        var faker = new Faker();
        var from = new TimeOnly(faker.Random.Int(min: 1, max: 10)); 
        userCalendarDay.AddTimeEvent(CalendarEventId.New(),  
            faker.Random.Word(),
            description ? faker.Lorem.Sentence() : null, 
            from, timeTo ? from.AddHours(1) : null);
        
        return userCalendarDay;
    }

    public static UserCalendarDay AddImportantDate(this UserCalendarDay userCalendarDay,
        bool description = false)
    {
        var faker = new Faker();
        userCalendarDay.AddImportantDate(CalendarEventId.New(),  
            faker.Random.Word(),
            description ? faker.Lorem.Sentence() : null);
        
        return userCalendarDay;
    }
}