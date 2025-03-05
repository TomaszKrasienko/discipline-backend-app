using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

internal sealed record ActivityRuleDocument : IDocument
{
    [BsonElement("id")]
    [BsonId]
    public required string Id { get; init; }
    
    [BsonElement("userId")] 
    public required string UserId { get; init; }
    
    [BsonElement("title")]
    public required string Title { get; init; }
    
    [BsonElement("note")]
    public string? Note { get; init; }
    
    [BsonElement("mode")] 
    public required string Mode { get; init; }
    
    [BsonElement("selectedDays")] 
    public IEnumerable<int>? SelectedDays{ get; init; }
    
    [BsonElement("stages")] 
    public IEnumerable<StageDocument>? Stages { get; init; }
}