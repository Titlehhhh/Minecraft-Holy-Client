using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefLoop : ProtodefType
	{
		[JsonPropertyName("endVal")]
		public uint EndValue { get; set; }

		[JsonPropertyName("type")]
		public ProtodefType Type { get; set; }
	}
	public sealed class ProtodefTopBitSetTerminatedArray : ProtodefType
	{
		[JsonPropertyName("type")]
		public ProtodefType Type { get; set; }
	}
}
