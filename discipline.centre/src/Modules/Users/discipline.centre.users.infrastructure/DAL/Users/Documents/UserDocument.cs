using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.users.infrastructure.DAL.Users.Documents;

internal sealed class UserDocument : IDocument
{
    [BsonId]
    [BsonElement("id")]
    public required string Id { get; set; }
    
    [BsonElement("email")]
    public required string Email { get; set; }
    
    [BsonElement("password")]
    public required string Password { get; set; }
    
    [BsonElement("firstName")]
    public required string FirstName { get; set; }
    
    [BsonElement("lastName")]
    public required string LastName { get; set; }
    
    [BsonElement("status")] 
    public required string Status { get; set; }
    
    [BsonElement("subscriptionOrder")]
    public SubscriptionOrderDocument? SubscriptionOrder { get; set; }
}