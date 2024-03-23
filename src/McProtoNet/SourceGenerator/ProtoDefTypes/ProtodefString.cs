using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefString : ProtodefType
	{
		[JsonPropertyName("countType")]
		public ProtodefType CountType { get; }

		

		[JsonConstructor]
		public ProtodefString(ProtodefType countType)
		{
			CountType = countType;
		}

	}


}
