using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

namespace discipline.centre.dailytrackers.infrastructure.DAL;

internal sealed class DailyTrackersMongoContext(IMongoClient mongoClient,
    IMongoCollectionNameConvention mongoCollectionNameConvention)
    : MongoCollectionContext(mongoClient, mongoCollectionNameConvention, "daily-trackers-module");