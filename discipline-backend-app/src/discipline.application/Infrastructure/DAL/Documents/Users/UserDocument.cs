using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents.Users;

public sealed class UserDocument : IDocument
{
    [BsonId]
    [BsonElement("id")]
    public string Id { get; set; }
    
    [BsonElement("email")]
    public string Email { get; set; }
    
    [BsonElement("password")]
    public string Password { get; set; }
    
    [BsonElement("firstName")]
    public string FirstName { get; set; }
    
    [BsonElement("lastName")]
    public string LastName { get; set; }
    
    [BsonElement("status")] 
    public string Status { get; set; }
    
    [BsonElement("subscriptionOrder")]
    public SubscriptionOrderDocument SubscriptionOrder { get; set; }
}