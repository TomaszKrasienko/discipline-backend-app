using discipline.centre.shared.abstractions.Serialization;
using discipline.centre.shared.infrastructure.Modules.Abstractions;
using Newtonsoft.Json;

namespace discipline.centre.shared.infrastructure.Modules;

internal sealed class ModuleTypesTranslator(
    ISerializer serializer) : IModuleTypesTranslator
{
    public object Translate(object value, Type type)
    {
        var sourceJson = serializer.ToJson(value);
        var targetInstance = serializer.ToObject(sourceJson, type);
        
        return targetInstance ?? throw new ArgumentException($"Can't convert value into type: {type.Name}");
    }

    public TResult Translate<TResult>(object value) where TResult : class
        => (TResult)Translate(value, typeof(TResult));
}