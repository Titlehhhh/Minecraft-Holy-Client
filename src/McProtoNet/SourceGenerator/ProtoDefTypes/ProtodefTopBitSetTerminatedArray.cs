using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefTopBitSetTerminatedArray : ProtodefType, IFieldsEnumerable
	{
		[JsonPropertyName("type")]
		public ProtodefType Type { get; set; }

		public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
		{
			yield return new("type", Type);
		}
	}
}
