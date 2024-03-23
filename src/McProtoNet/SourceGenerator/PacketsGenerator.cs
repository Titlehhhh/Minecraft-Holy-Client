using SourceGenerator.ProtoDefTypes;

public class PacketsGenerator
{
	private readonly Dictionary<string, ProtodefType> packets;

	public PacketsGenerator(Dictionary<string, ProtodefType> packets)
	{
		this.packets = packets;
	}

	public async Task Generate(string dir)
	{

	}
}
