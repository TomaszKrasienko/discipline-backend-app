using MongoDB.Bson.Serialization.Attributes;

namespace discipline.infrastructure.DAL.Documents.DailyProductivities;

internal sealed class DailyProductivityDocument : IDocument
{
    [BsonElement("id")]
    [BsonId]
    public required string Id { get; set; }
    
    [BsonElement("day")]
    public DateOnly Day { get; set; }
    
    [BsonElement("userId")] 
    public required string UserId { get; set; }

    [BsonElement("activities")]
    public IEnumerable<ActivityDocument>? Activities { get; set; }
}