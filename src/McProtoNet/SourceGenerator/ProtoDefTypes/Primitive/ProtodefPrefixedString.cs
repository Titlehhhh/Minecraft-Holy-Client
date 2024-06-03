using SourceGenerator.ProtoDefTypes.Attributes;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{
	
	public sealed class ProtodefPrefixedString : ProtodefType
	{
		[JsonPropertyName("countType")]
		public ProtodefType CountType { get; }



		[JsonConstructor]
		public ProtodefPrefixedString(ProtodefType countType)
		{
			CountType = countType;
		}
		public override string? GetNetType()
		{
			return "string";
		}
	}


}
