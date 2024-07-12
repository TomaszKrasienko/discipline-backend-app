using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents;

public class EventDocument
{
    [BsonId] 
    [BsonElement("id")]
    public Guid Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }
    
    
}