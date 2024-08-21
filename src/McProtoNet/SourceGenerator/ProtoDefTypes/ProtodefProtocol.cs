using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public class ProtodefProtocol : IJsonOnDeserialized, ICloneable
{
    [JsonConstructor]
    public ProtodefProtocol()
    {
        Namespaces = new Dictionary<string, ProtodefNamespace>();
    }

    private ProtodefProtocol(ProtodefProtocol other)
    {
        Namespaces = other.Namespaces
            .Select(x => new KeyValuePair<string, ProtodefNamespace>(x.Key, (ProtodefNamespace)x.Value.Clone()))
            .ToDictionary();

        Types = other.Types
            .Select(x => new KeyValuePair<string, ProtodefType>(x.Key, (ProtodefType)x.Value.Clone()))
            .ToDictionary();
    }

    [JsonPropertyName("types")] public Dictionary<string, ProtodefType> Types { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement> AdditionalData { get; set; } = new Dictionary<string, JsonElement>();

    [JsonIgnore] public Dictionary<string, ProtodefNamespace> Namespaces { get; }

    public object Clone()
    {
        return new ProtodefProtocol(this);
    }

    public ProtodefNamespace this[string path]
    {
        get
        {
            var paths = path.Split(".");

            if (paths.Length == 1)
            {
                return Namespaces[path];
            }

            var f = paths.First();


            ProtodefNamespace ns = Namespaces[f];

            for (int i = 1; i < paths.Length; i++)
            {
                string item = paths[i];
                ns = (ProtodefNamespace)ns.Types[item];
            }

            return ns;
        }
    }

    public void OnDeserialized()
    {
        foreach (var (key, value) in AdditionalData)
        {
            var namespaceObj = ParseNamespace(value);
            Namespaces[key] = namespaceObj;
        }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        Console.WriteLine("GetObjectData");
    }

    private ProtodefNamespace ParseNamespace(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            var types = new Dictionary<string, ProtodefType>();
            foreach (var item in element.EnumerateObject())
            {
                if (item.NameEquals("types"))
                {
                    types = item.Value.Deserialize<Dictionary<string, ProtodefType>>();
                    break;
                }

                var namespaceObj = ParseNamespace(item.Value);
                types[item.Name] = namespaceObj;
            }

            return new ProtodefNamespace { Types = types };
        }

        throw new JsonException("Invalid namespace format.");
    }
}