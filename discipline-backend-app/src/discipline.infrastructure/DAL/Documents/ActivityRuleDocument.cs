using MongoDB.Bson.Serialization.Attributes;

namespace discipline.infrastructure.DAL.Documents;

public sealed class ActivityRuleDocument : IDocument
{
    [BsonElement("id")]
    [BsonId]
    public string Id { get; init; }
    
    [BsonElement("userId")] 
    public string UserId { get; set; }
    
    [BsonElement("title")]
    public string Title { get; init; }
    
    [BsonElement("mode")] 
    public string Mode { get; init; }
    [BsonElement("selectedDays")] 
    public IEnumerable<int> SelectedDays{ get; init; }
}