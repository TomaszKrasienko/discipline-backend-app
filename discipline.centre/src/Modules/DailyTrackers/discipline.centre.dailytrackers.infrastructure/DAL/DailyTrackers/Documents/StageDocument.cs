using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

public sealed record StageDocument : IDocument
{
    [BsonId]
    [BsonElement("stageId")]
    public required string StageId { get; init; }
    
    [BsonElement("title")]
    public required string Title { get; init; }
    
    [BsonElement("index")]
    public int Index { get; init; }
    
    [BsonElement("isChecked")]
    public bool IsChecked { get; init; }
}