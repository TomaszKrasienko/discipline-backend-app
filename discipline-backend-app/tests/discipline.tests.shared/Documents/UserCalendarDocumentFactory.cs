using Bogus;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;

namespace discipline.tests.shared.Documents;

internal static class UserCalendarDocumentFactory
{
    internal static UserCalendarDocument Get(IEnumerable<EventDocument> eventDocuments)
        => Get(1, eventDocuments).Single();
    
    private static List<UserCalendarDocument> Get(int count, IEnumerable<EventDocument> eventDocuments)
        => GetFaker(eventDocuments).Generate(count);
    
    private static Faker<UserCalendarDocument> GetFaker(IEnumerable<EventDocument> eventDocuments)
        => new Faker<UserCalendarDocument>()
            .RuleFor(f => f.Day, v => new DateOnly(2024, 1, 1))
            .RuleFor(f => f.UserId, v => Guid.NewGuid())
            .RuleFor(f => f.Events, v => eventDocuments);
}