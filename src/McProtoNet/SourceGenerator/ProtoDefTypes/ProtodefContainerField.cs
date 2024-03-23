using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefContainerField : ProtodefType
	{

		[JsonConstructor]
		public ProtodefContainerField(bool? anon, string? name, ProtodefType type)
		{
			Anon = anon;
			Name = name;
			Type = type;
		}

		//public bool IsAnon => Anon;

		[JsonPropertyName("anon")]
		public bool? Anon { get; }
		[JsonPropertyName("name")]
		public string? Name { get; }
		[JsonPropertyName("type")]
		public ProtodefType Type { get; }
	}


}
