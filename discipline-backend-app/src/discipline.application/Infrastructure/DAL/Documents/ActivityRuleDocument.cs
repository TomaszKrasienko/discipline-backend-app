using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents;

public sealed class ActivityRuleDocument : IDocument
{
    [BsonElement("id")]
    [BsonId]
    public string Id { get; init; }
    
    [BsonElement("userId")] 
    public Ulid UserId { get; set; }
    
    [BsonElement("title")]
    public string Title { get; init; }
    
    [BsonElement("mode")] 
    public string Mode { get; init; }
    [BsonElement("selectedDays")] 
    public IEnumerable<int> SelectedDays{ get; init; }
}