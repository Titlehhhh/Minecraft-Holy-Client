
namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefOption : ProtodefType, IPathTypeEnumerable
	{
		public ProtodefType Type { get; }

		public ProtodefOption(ProtodefType type)
		{
			Type = type;
		}

		public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
		{
			if (Type is IPathTypeEnumerable)
				yield return new("type", Type);
		}

		public override string? GetNetType()
		{
			string? netType = Type.GetNetType();
			if(netType is not null)
			{
				return netType + "?";
			}
			return null;
		}
	}
}
