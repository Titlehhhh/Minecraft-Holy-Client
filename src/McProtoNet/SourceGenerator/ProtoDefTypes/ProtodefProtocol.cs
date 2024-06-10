using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{

	public class ProtocolConverter : JsonConverter<ProtodefProtocol>
	{
		public override ProtodefProtocol? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}

		public override void Write(Utf8JsonWriter writer, ProtodefProtocol value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}

	public class ProtodefProtocol : IJsonOnDeserialized
	{

		[JsonPropertyName("types")]
		public Dictionary<string, ProtodefType> Types { get; set; }

		[JsonExtensionData]
		public IDictionary<string, JsonElement> AdditionalData { get; set; } = new Dictionary<string, JsonElement>();

		[JsonIgnore]
		public Dictionary<string, Namespace> Namespaces { get; private set; }

		[JsonConstructor]
		public ProtodefProtocol()
		{
			Namespaces = new Dictionary<string, Namespace>();
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			System.Console.WriteLine("GetObjectData");

		}


		public void OnDeserialized()
		{
			foreach (var (key, value) in AdditionalData)
			{

				var namespaceObj = ParseNamespace(value);
				Namespaces[key] = namespaceObj;

			}

		}

		private Namespace ParseNamespace(JsonElement element)
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
				return new Namespace() { Types = types };
			}
			throw new JsonException("Invalid namespace format.");
		}
	}
}
