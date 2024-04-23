using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace theredhead.remoting.serialization;

public class JsonSerializer : ISerializer
{
    public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        MaxDepth = 50,
        MissingMemberHandling = MissingMemberHandling.Error,
        TypeNameHandling = TypeNameHandling.Auto,
        DateParseHandling = DateParseHandling.DateTime,
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Include,
        ConstructorHandling = ConstructorHandling.Default,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public T Deserialize<T>(string serializedSubject)
    {
        #pragma warning disable CS8603 // Possible null reference return.
        return JsonConvert.DeserializeObject<T>(serializedSubject);
        #pragma warning restore CS8603 // Possible null reference return.
    }

    public string Serialize<T>(T subject)
    {
        return JsonConvert.SerializeObject(subject, SerializerSettings);
    }

    public void Dispose()
    {
    }
}


/// <summary>
/// Utility class for quick and dirty JSON serialization and deserialization
/// </summary>
public static class JSON 
{
    private static JsonSerializer Serializer { get; } = new JsonSerializer();

    public static string Stringify<T>(T subject) => Serializer.Serialize(subject);
    public static T Parse<T>(string serializedSubject) => Serializer.Deserialize<T>(serializedSubject);
}