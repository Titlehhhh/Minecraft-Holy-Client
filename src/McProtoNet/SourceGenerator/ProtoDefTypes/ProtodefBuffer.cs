using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefBuffer : ProtodefType
	{
		[JsonConstructor]
		public ProtodefBuffer(ProtodefType countType, string? count, bool? rest)
		{
			CountType = countType;
			Count = count;
			Rest = rest;
		}

		[JsonPropertyName("countType")]
		public ProtodefType CountType { get; }

		[JsonPropertyName("count")]
		public string? Count { get; }
		[JsonPropertyName("rest")]
		public bool? Rest { get; }
	}


}
