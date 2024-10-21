using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents.UsersCalendar;

public class UserCalendarDocument : IDocument
{
    [BsonElement("id")]
    [BsonId]
    public Ulid Id { get; set; }
    
    [BsonElement("day")]
    public DateOnly Day { get; set; }
    
    [BsonElement("userId")] 
    public Ulid UserId { get; set; }
    
    [BsonElement("events")] 
    public IEnumerable<EventDocument> Events { get; set; }
}