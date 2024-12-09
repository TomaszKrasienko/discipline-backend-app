using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

internal sealed record StageDocument : IDocument
{
    [BsonElement("id")]
    [BsonId]
    public required string StageId { get; init; }
    
    [BsonElement("title")] 
    public required string Title { get; init; }
    
    [BsonElement("index")] 
    public int Index { get; init; }
}