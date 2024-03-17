namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefNumericType : ProtodefType
	{
		public string Name { get; }

		public bool Signed { get; }

		public ByteOrder Order { get; }

		public ProtodefNumericType(string name, bool signed, ByteOrder order)
		{
			Name = name;
			Signed = signed;
			Order = order;
		}
	}


}
