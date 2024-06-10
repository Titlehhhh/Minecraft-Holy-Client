using SourceGenerator.ProtoDefTypes;
using System.Diagnostics;
using System.Text;
using Humanizer;

public sealed class ProtocolSourceGenerator
{
	public Protocol Protocol;
	public string Version;
	public int ProtocolVersion;





	public Dictionary<string, string> Generate()
	{
		Dictionary<string, string> result = new();


		foreach ((string nsName, Namespace side) in Protocol.Namespaces)
		{
			var serverPackets = side.Types["toClient"] as Namespace;
			var clientPackets = side.Types["toServer"] as Namespace;

			if (nsName == "play")
			{
				
			}
		}

		return result;

	}
	private static int AnonId = 0;
	private List<string[]> GenerateSendMethods(Dictionary<string, ProtodefType> types)
	{

		List<string[]> result = new();

		ProtodefType IdMap = types["packet"];


		foreach ((string name, ProtodefType type) in types)
		{
			if (name != "packet")
			{

				bool isSkip = false;
				ProtodefContainer fields = type as ProtodefContainer;

			}
		}

		return result;
	}


	
	

	private static string FieldToNetType(ProtodefContainerField field)
	{
		return field.Type.GetNetType();
	}

}



public static class Mappings
{

}

