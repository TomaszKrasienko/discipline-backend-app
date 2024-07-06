using Newtonsoft.Json;

namespace discipline.application.Behaviours;

internal static class JsonSerializationBehaviour
{
    internal static string AsJson(this object data)
        => JsonConvert.SerializeObject(data);
}
