namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefOption : ProtodefType
	{
		public ProtodefType Type { get; }

		public ProtodefOption(ProtodefType type)
		{
			Type = type;
		}
	}
}
