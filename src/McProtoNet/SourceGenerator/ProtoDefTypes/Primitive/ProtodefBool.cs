using SourceGenerator.ProtoDefTypes.Attributes;

namespace SourceGenerator.ProtoDefTypes
{
	
	public sealed class ProtodefBool : ProtodefType
	{
		public override string ToString()
		{
			return "bool";
		}

		public override string? GetNetType()
		{
			return "bool";
		}
	}


}
