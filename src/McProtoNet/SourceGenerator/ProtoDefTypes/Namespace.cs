namespace SourceGenerator.ProtoDefTypes
{

	public sealed class Namespace : ProtodefType
	{

		public Dictionary<string, ProtodefType> Types { get; set; } = new();

	}
}
