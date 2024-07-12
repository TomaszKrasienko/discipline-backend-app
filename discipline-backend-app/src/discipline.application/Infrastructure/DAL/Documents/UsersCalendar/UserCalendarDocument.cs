using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents.UsersCalendar;

public class UserCalendarDocument : IDocument
{
    [BsonElement("day")]
    [BsonId]
    public DateOnly Day { get; set; }
    
    [BsonElement("events")] 
    public IEnumerable<EventDocument> Type { get; set; }
}