using System.Linq.Expressions;
using MongoDB.Driver;

namespace discipline.infrastructure.DAL;

internal static class FilterDefinitionBuilderExtension
{
    internal static FilterDefinition<T> ToFilterDefinition<T>(this Expression<Func<T, bool>> expression) where T : class
        =>  Builders<T>.Filter.Where(expression);
}