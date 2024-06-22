using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents;

public sealed class ActivityRuleDocument
{
    [BsonId]
    public Guid Id { get; init; }
    
    [BsonElement("title")]
    public string Title { get; init; }
    
    [BsonElement("mode")] 
    public string Mode { get; init; }
    
    [BsonElement("selectedDays")] 
    public IEnumerable<int> SelectedDays{ get; init; }
}