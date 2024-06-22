using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents;

public sealed class ActivityRuleDocument
{
    [BsonId]
    public Guid Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }
    
    [BsonElement("mode")] 
    public string Mode { get; set; }
    
    [BsonElement("selectedDays")] 
    public IEnumerable<int> SelectedDays{ get; set; }
}