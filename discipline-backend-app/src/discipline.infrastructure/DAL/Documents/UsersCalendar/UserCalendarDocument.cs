using MongoDB.Bson.Serialization.Attributes;

namespace discipline.infrastructure.DAL.Documents.UsersCalendar;

public class UserCalendarDocument : IDocument
{
    [BsonElement("id")]
    [BsonId]
    public string Id { get; set; }
    
    [BsonElement("day")]
    public DateOnly Day { get; set; }
    
    [BsonElement("userId")] 
    public string UserId { get; set; }
    
    [BsonElement("events")] 
    public IEnumerable<EventDocument> Events { get; set; }
}