using discipline.centre.shared.abstractions.Serialization;
using discipline.centre.shared.infrastructure.Modules.Abstractions;

namespace discipline.centre.shared.infrastructure.Modules;

internal sealed class ModuleTypesTranslator(
    ISerializer serializer) : IModuleTypesTranslator
{
    public object Translate(object value, Type type)
    {
        var sourceJson = serializer.ToJson(value);
        return serializer.ToObject(sourceJson, type)
               ?? throw new ArgumentException($"Can't convert value into type: {type.Name}");
    }

    public TResult Translate<TResult>(object value) where TResult : class
    {
        var sourceJson = serializer.ToJson(value);
        return serializer.ToObject<TResult>(sourceJson) 
               ?? throw new ArgumentException($"Can't convert value into type: {typeof(TResult).Name}");
    }
}