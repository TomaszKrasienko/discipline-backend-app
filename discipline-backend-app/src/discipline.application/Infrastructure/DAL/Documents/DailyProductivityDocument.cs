using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents;

public class DailyProductivityDocument : IDocument
{
    [BsonElement("id")]
    [BsonId]
    public string Id { get; set; }
    
    [BsonElement("day")]
    public DateOnly Day { get; set; }
    
    [BsonElement("userId")] 
    public Ulid UserId { get; set; }

    [BsonElement("activities")]
    public IEnumerable<ActivityDocument> Activities { get; set; }
}