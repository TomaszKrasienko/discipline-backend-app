using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents.Users;

[BsonDiscriminator]
[BsonKnownTypes(typeof(FreeSubscriptionOrderDocument), typeof(PaidSubscriptionOrderDocument))]
public class SubscriptionOrderDocument : IDocument
{
    [BsonId]
    [BsonElement("id")]
    public Ulid Id { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("subscriptionId")]
    public Ulid SubscriptionId { get; set; }
    
    [BsonElement("stateIsCancelled")]
    public bool StateIsCancelled { get; set; }
    
    [BsonElement("stateActiveTill")]
    public DateOnly? StateActiveTill { get; set; }
    

}