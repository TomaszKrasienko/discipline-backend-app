using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents.UsersCalendar;

public class UserCalendarDocument : IDocument
{
    [BsonElement("day")]
    [BsonId]
    public DateOnly Day { get; set; }
    
    [BsonElement("userId")] 
    public Guid UserId { get; set; }
    
    [BsonElement("events")] 
    public IEnumerable<EventDocument> Events { get; set; }
}