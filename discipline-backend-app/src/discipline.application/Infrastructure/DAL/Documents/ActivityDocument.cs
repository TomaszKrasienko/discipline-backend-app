using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents;

public class ActivityDocument : IDocument
{
    [BsonId] 
    [BsonElement("id")]
    public Guid Id { get; set; }
    
    [BsonElement("title")] 
    public string Title { get; set; }
    
    [BsonElement("isChecked")] 
    public bool IsChecked { get; set; }
    
    [BsonElement("parentRuleId")]
    public Guid? ParentRuleId { get; set; }
}