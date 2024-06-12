namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefNumericType : ProtodefType
	{
		public string NetName { get; }

		public string OriginalName { get; }

		public bool Signed { get; }

		public ByteOrder Order { get; }

		public ProtodefNumericType(string name,string originalName, bool signed, ByteOrder order)
		{
			NetName = name;
			OriginalName = originalName;
			Signed = signed;
			Order = order;
		}
		public override string ToString()
		{
			return NetName;
		}

		public override string? GetNetType()
		{
			return NetName;
		}
	}


}
