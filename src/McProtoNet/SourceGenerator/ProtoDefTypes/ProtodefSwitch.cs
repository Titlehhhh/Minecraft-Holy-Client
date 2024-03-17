using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefSwitch : ProtodefType
	{


		[JsonIgnore]
		public string? Owner { get; set; }

		//TODO path parser
		[JsonPropertyName("compareTo")]
		public string CompareTo { get; set; }

		[JsonPropertyName("compareToValue")]
		public string? CompareToValue { get; set; }

		[JsonPropertyName("fields")]
		public Dictionary<string, ProtodefType> Fields { get; set; } = new();

		[JsonPropertyName("default")]
		public ProtodefType? Default { get; set; }
	}


}
