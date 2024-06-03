namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefVarLong : ProtodefType
	{
		public override string ToString()
		{
			return "varlong";
		}

		public override string? GetNetType()
		{
			return "long";
		}
	}


}
