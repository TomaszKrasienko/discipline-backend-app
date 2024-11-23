using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

namespace discipline.centre.users.infrastructure.DAL;

internal sealed class UsersMongoContext(
    IMongoClient mongoClient, 
    IMongoCollectionNameConvention mongoCollectionNameConvention) 
        : MongoCollectionContext(mongoClient, mongoCollectionNameConvention, "users-module")
{
    
}
