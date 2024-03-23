
namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefOption : ProtodefType, IFieldsEnumerable
	{
		public ProtodefType Type { get; }

		public ProtodefOption(ProtodefType type)
		{
			Type = type;
		}

		public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
		{
			yield return new("type", Type);
		}
	}
}
