using discipline.application.Infrastructure.DAL.Connection;
using MongoDB.Driver;
using Xunit;

namespace discipline.api.integration_tests._Helpers;

internal class CollectionProvider : IClassFixture<IDisciplineMongoCollection>
{
    IMongoCollection<T> GetCollection()
}