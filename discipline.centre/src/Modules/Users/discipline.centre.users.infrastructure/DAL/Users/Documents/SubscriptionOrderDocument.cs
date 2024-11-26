using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.users.infrastructure.DAL.Users.Documents;

[BsonDiscriminator]
[BsonKnownTypes(typeof(FreeSubscriptionOrderDocument), typeof(PaidSubscriptionOrderDocument))]
internal abstract class SubscriptionOrderDocument : IDocument
{
    [BsonId]
    [BsonElement("id")]
    public required string Id { get; set; }
    
    [BsonElement("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }
    
    [BsonElement("subscriptionId")]
    public Ulid SubscriptionId { get; set; }
    
    [BsonElement("stateIsCancelled")]
    public bool StateIsCancelled { get; set; }
    
    [BsonElement("stateActiveTill")]
    public DateOnly? StateActiveTill { get; set; }
    

}