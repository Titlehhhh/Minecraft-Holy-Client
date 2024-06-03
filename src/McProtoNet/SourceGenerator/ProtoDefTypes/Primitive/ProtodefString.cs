namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefString : ProtodefType
	{
		public override string ToString()
		{
			return "string";
		}

		public override string? GetNetType()
		{
			return "string";
		}
	}


}
