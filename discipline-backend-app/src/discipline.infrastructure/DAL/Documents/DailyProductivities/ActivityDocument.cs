using MongoDB.Bson.Serialization.Attributes;

namespace discipline.infrastructure.DAL.Documents.DailyProductivities;

public class ActivityDocument : IDocument
{
    [BsonId] 
    [BsonElement("id")]
    public required string Id { get; set; }
    
    [BsonElement("title")] 
    public required string Title { get; set; }
    
    [BsonElement("isChecked")] 
    public required bool IsChecked { get; set; }
    
    [BsonElement("parentRuleId")]
    public string? ParentRuleId { get; set; }
}