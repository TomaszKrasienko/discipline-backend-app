using MongoDB.Bson.Serialization.Attributes;

namespace discipline.infrastructure.DAL.Documents.ActivityRules;

internal sealed class ActivityRuleDocument : IDocument
{
    [BsonElement("id")]
    [BsonId]
    public required string Id { get; init; }
    
    [BsonElement("userId")] 
    public required string UserId { get; set; }
    
    [BsonElement("title")]
    public required string Title { get; init; }
    
    [BsonElement("mode")] 
    public required string Mode { get; init; }
    
    [BsonElement("selectedDays")] 
    public IEnumerable<int>? SelectedDays{ get; init; }
}