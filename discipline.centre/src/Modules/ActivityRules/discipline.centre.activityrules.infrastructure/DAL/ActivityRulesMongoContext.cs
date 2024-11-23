using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

namespace discipline.centre.activityrules.infrastructure.DAL;

internal sealed class ActivityRulesMongoContext(
    IMongoClient mongoClient, 
    IMongoCollectionNameConvention mongoCollectionNameConvention) 
    : MongoCollectionContext(mongoClient, mongoCollectionNameConvention, "activity-rules-module")
{
    
}