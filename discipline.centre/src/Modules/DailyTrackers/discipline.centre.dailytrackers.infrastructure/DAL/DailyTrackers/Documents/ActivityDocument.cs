using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

public sealed record ActivityDocument : IDocument
{
    [BsonId]
    [BsonElement("activityId")]
    public required string ActivityId { get; init; }
    
    [BsonElement("title")] 
    public required string Title { get; init; }
    
    [BsonElement("note")] 
    public string? Note { get; init; }

    [BsonElement("isChecked")]
    public bool IsChecked { get; init; }

    [BsonElement("parentActivityRuleId")]
    public string? ParentActivityRuleId { get; init; }

    [BsonElement("stages")]
    public IEnumerable<StageDocument>? Stages { get; set; }
}