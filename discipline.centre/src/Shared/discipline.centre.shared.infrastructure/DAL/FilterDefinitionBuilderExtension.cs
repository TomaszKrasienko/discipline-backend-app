using MongoDB.Driver;

// ReSharper disable once CheckNamespace
namespace System.Linq.Expressions;

public static class FilterDefinitionBuilderExtension
{
    public static FilterDefinition<T> ToFilterDefinition<T>(this Expression<Func<T, bool>> expression) where T : class
        =>  Builders<T>.Filter.Where(expression);
}