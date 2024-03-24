using SourceGenerator.ProtoDefTypes;

public interface ISourceGenerator
{
	void Reset(ProtodefType protodefType);

	string GenerateNetClass();
	string GenerateReadMethod();
	string GenerateWriteMethod();
}

public sealed class BitFieldGenerator : ISourceGenerator
{
	private ProtodefBitField _bitField;

	public BitFieldGenerator()
	{

	}
	public string GenerateNetClass()
	{
		return null;
	}

	public string GenerateReadMethod()
	{
		throw new NotImplementedException();
	}

	public string GenerateWriteMethod()
	{
		throw new NotImplementedException();
	}

	public void Reset(ProtodefType protodefType)
	{
		if(protodefType is ProtodefBitField bitField)
		{
			_bitField = bitField;
		}
		throw new ArgumentException();
	}
}