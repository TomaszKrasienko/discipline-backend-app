namespace discipline.centre.shared.infrastructure.Modules.Abstractions;

/// <summary>
/// Interface for translating values into provided types
/// </summary>
internal interface IModuleTypesTranslator
{
    /// <summary>
    /// Translates an object into the specified type
    /// </summary>
    /// <param name="value">Object to be mapped</param>
    /// <param name="type">The target type for the translation</param>
    /// <returns>The translated type</returns>
    object Translate(object value, Type type);
    
    /// <summary>
    /// Translates an object into the specified type
    /// </summary>
    /// <param name="value">Object to be mapped</param>
    /// <typeparam name="TResult">The target type for the translation</typeparam>
    /// <returns>The translated type</returns>
    TResult Translate<TResult>(object value) where TResult : class;
}